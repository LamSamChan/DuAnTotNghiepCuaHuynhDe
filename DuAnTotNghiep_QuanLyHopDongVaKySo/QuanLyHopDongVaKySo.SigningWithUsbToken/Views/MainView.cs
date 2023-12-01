using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumViewer;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Repository;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using Org.BouncyCastle.Crypto.Tls;
using System.Diagnostics;
using iTextSharp.text.pdf.security;
using iTextSharp.text.pdf;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Models;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    public partial class MainView : Form
    {
        private ContractRepository contractRepository = new ContractRepository();
        private MinuteRepository minuteRepository = new MinuteRepository();
        private TypeOfServiceRepository tosRepository = new TypeOfServiceRepository();
        private TContractRepository tContractRepository = new TContractRepository();
        private SendDContractRepository sendDContractRepository = new SendDContractRepository();
        public MainView()
        {
            InitializeComponent();
            TypeDocument.SelectedIndex = 0;
        }

        private async void getContractButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (TypeDocument.SelectedItem.ToString() == "Hợp đồng")
                {
                    string customerId = DataStore.Instance.Customer.CustomerId.ToString();
                    int contractId = Convert.ToInt32(inputContractId.Text);
                    var pContract = await contractRepository.GetPContractById(customerId, contractId);

                    if (pContract.PContractID != null)
                    {
                        DataStore.Instance.PendingContract = pContract;
                        byte[] pdfBytes = Convert.FromBase64String(pContract.Base64File);
                        DataStore.Instance.SavePath = Path.Combine(Application.StartupPath, "AppData", $"{pContract.PContractName}.pdf");
                        SaveByteArrayToFile(DataStore.Instance.SavePath, pdfBytes);

                        DataStore.Instance.SavePathSign = Path.Combine(Application.StartupPath, "AppData", $"{pContract.PContractName}_sign.pdf");
                        SaveByteArrayToFile(DataStore.Instance.SavePathSign, pdfBytes);

                        var stream = new MemoryStream(pdfBytes);
                        PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer.Document = pdfDocument;

                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hợp đồng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    int minuteId = Convert.ToInt32(inputContractId.Text);
                    var pMinute = await minuteRepository.GetPMinuteById(minuteId);
                    if (pMinute != null)
                    {
                        byte[] pdfBytes = Convert.FromBase64String(pMinute.Base64File);
                        DataStore.Instance.SavePath = Path.Combine(Application.StartupPath, "AppData", $"{pMinute.MinuteName}.pdf");
                        SaveByteArrayToFile(DataStore.Instance.SavePath, pdfBytes);

                        var stream = new MemoryStream(pdfBytes);
                        PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer.Document = pdfDocument;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy hợp đồng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã hợp đồng phải là số !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SaveByteArrayToFile(string filePath, byte[] byteArray)
        {
            // Mở hoặc tạo tệp để lưu trữ
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                // Ghi dữ liệu vào tệp
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }

        private async void signContract_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại cho phép chọn giữa hai options
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn ký bằng chữ chữ ký không?", "Lựa chọn kiểu chữ ký", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes) // Option A
            {
                OpenImageFileDialog();
            }
            else // Option B
            {
                var tos = await tosRepository.GetTypeOfServiceAsync(DataStore.Instance.PendingContract.TOS_ID);
                var tcontract = await tContractRepository.GetTContact(tos.templateContractID);
                SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tcontract.jsonCustomerZone);

                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);

                X509Certificate2 cert = sel[0];

                Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] {
                    cp.ReadCertificate(cert.RawData)};

                IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");

                string savePathsign = DataStore.Instance.SavePathSign;
                PdfReader pdfReader = new PdfReader(savePathsign);

                FileStream signedPdf = new FileStream(savePathsign.Replace("_sign.pdf", "_signed.pdf"), FileMode.Create);

                PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                signatureAppearance.SignDate = DateTime.Now;
                signatureAppearance.SignatureCreator = DataStore.Instance.Customer.FullName;

                float x = customerZone.X - 60;
                float y = customerZone.Y - 100;
                signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 170, y + 170), pdfReader.NumberOfPages, "Signature");

                signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

                byte[] pdfBytes = File.ReadAllBytes(savePathsign.Replace("_sign.pdf", "_signed.pdf"));
                var stream = new MemoryStream(pdfBytes);
                PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                pdfViewer.Document = pdfDocument;

                MessageBox.Show("Đã ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string serial = null;
        private async void OpenImageFileDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn ảnh chữ ký";
                openFileDialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Xử lý ảnh đã chọn
                    string selectedFilePath = openFileDialog.FileName;

                    var tos = await tosRepository.GetTypeOfServiceAsync(DataStore.Instance.PendingContract.TOS_ID);
                    var tcontract = await tContractRepository.GetTContact(tos.templateContractID);
                    SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tcontract.jsonCustomerZone);

                    X509Store store = new X509Store(StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);
                    if (sel.Count > 0)
                    {
                        X509Certificate2 cert = sel[0];

                        Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                        Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] {
                    cp.ReadCertificate(cert.RawData)};

                        IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");

                        string savePathsign = DataStore.Instance.SavePathSign;
                        PdfReader pdfReader = new PdfReader(savePathsign);

                        FileStream signedPdf = new FileStream(savePathsign.Replace("_sign.pdf", "_signed.pdf"), FileMode.Create);

                        PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
                        PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

                        signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(selectedFilePath);
                        float x = customerZone.X - 60;
                        float y = customerZone.Y - 100;
                        signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 170, y + 170), pdfReader.NumberOfPages, "Signature");
                        signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;

                        MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

                        byte[] pdfBytes = File.ReadAllBytes(savePathsign.Replace("_sign.pdf", "_signed.pdf"));
                        var stream = new MemoryStream(pdfBytes);
                        PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer.Document = pdfDocument;

                        MessageBox.Show("Đã ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Bạn chưa chọn chứng chỉ nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private async void submitSign_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hoàn thành ký?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           
            if (result == DialogResult.Yes)
            {
                pdfViewer.Document.Dispose();
                string savePathsign = DataStore.Instance.SavePathSign;
                FileStream fsPContract1 = new System.IO.FileStream(savePathsign.Replace("_sign.pdf", "_signed.pdf"), FileMode.Open, FileAccess.Read);
                fsPContract1.Close();
                byte[] fileBytes = System.IO.File.ReadAllBytes(savePathsign.Replace("_sign.pdf", "_signed.pdf"));
                string base64String = Convert.ToBase64String(fileBytes);
                DoneContract doneContract = new DoneContract() { 
                    DConTractName = DataStore.Instance.PendingContract.PContractName,
                    PContractID = int.Parse(DataStore.Instance.PendingContract.PContractID),
                    Base64File = base64String,
                };
                int isSuccess = await sendDContractRepository.PostContract(doneContract);
                if (isSuccess != 0)
                {
                    MessageBox.Show("Đã đã hoàn tất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đã có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
