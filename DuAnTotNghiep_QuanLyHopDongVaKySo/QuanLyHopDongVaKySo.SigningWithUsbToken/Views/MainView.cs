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
using System.Diagnostics.Eventing.Reader;
using QuanLyHopDongVaKySo.SigningWithUsbTokenI.Models;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    public partial class MainView : Form
    {
        private ContractRepository contractRepository = new ContractRepository();
        private MinuteRepository minuteRepository = new MinuteRepository();
        private TypeOfServiceRepository tosRepository = new TypeOfServiceRepository();
        private TContractRepository tContractRepository = new TContractRepository();
        private TMinuteRepository tMinuteRepository = new TMinuteRepository();
        private DContractRepository dContractRepository = new DContractRepository();
        private SendDContractRepository sendDContractRepository = new SendDContractRepository();
        private SendDMinuteRepository sendDMinuteRepository = new SendDMinuteRepository();


        public MainView()
        {
            InitializeComponent();
            TypeDocument.SelectedIndex = 0;
        }

        private async void getContractButton_Click(object sender, EventArgs e)
        {
            try
            {
                string customerId = DataStore.Instance.Customer.CustomerId.ToString();
                if (TypeDocument.SelectedItem.ToString() == "Hợp đồng")
                {
                    int contractId = Convert.ToInt32(inputContractId.Text);
                    var pContract = await contractRepository.GetPContractById(customerId, contractId);

                    if (pContract.PContractID != null)
                    {
                        DataStore.Instance.PendingContract = pContract;
                        byte[] pdfBytes = Convert.FromBase64String(pContract.Base64File);
                        DataStore.Instance.SavePath = Path.Combine(Application.StartupPath, "AppData", $"{pContract.PContractName}.pdf");

                        if (!Directory.Exists(Path.Combine(Application.StartupPath, "AppData")))
                        {
                            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "AppData"));
                        }

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
                    var pMinute = await minuteRepository.GetPMinuteById(customerId, minuteId);
                    if (pMinute.PendingMinuteId != null)
                    {
                        DataStore.Instance.PendingMinute = pMinute;
                        byte[] pdfBytes = Convert.FromBase64String(pMinute.Base64File);
                        DataStore.Instance.SavePath = Path.Combine(Application.StartupPath, "AppData", $"{pMinute.MinuteName}.pdf");

                        if (!Directory.Exists(Path.Combine(Application.StartupPath, "AppData")))
                        {
                            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "AppData"));
                        }

                        SaveByteArrayToFile(DataStore.Instance.SavePath, pdfBytes);

                        DataStore.Instance.SavePathSign = Path.Combine(Application.StartupPath, "AppData", $"{pMinute.MinuteName}_sign.pdf");
                        SaveByteArrayToFile(DataStore.Instance.SavePathSign, pdfBytes);

                        var stream = new MemoryStream(pdfBytes);
                        PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                        pdfViewer.Document = pdfDocument;
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy biên bản !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tìm thấy tài liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn ký bằng ảnh chữ ký không?", "Lựa chọn kiểu chữ ký", MessageBoxButtons.YesNo);
            float x, y;
            string reason;
            if (TypeDocument.SelectedItem.ToString() == "Hợp đồng")
            {
                var tos = await tosRepository.GetTypeOfServiceAsync(DataStore.Instance.PendingContract.TOS_ID);
                var tcontract = await tContractRepository.GetTContact(tos.templateContractID);
                SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tcontract.jsonCustomerZone);
                reason = "Ký hợp đồng";
                x = customerZone.X - 30;
                y = customerZone.Y - 55;
            }
            else
            {
                reason = "Ký biên bản lắp đặt";
                var dContract = await dContractRepository.GetDoneContract(DataStore.Instance.PendingMinute.DoneContractId.ToString());
                var tos = await tosRepository.GetTypeOfServiceAsync(dContract.TOS_ID.ToString());
                int tMinuteID = tos.templateMinuteID;
                TemplateMinute tMinute = await tMinuteRepository.GetTContact(tMinuteID);
                SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tMinute.jsonCustomerZone);
                x = customerZone.X - 110;
                y = customerZone.Y - 850;
            }

            if (dialogResult == DialogResult.Yes) // Option A
            {
                OpenImageFileDialog(x, y, reason);
            }
            else // Option B
            {
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);

                X509Certificate2 cert = new X509Certificate2();
                try
                {
                    cert = sel[0];
                }
                catch (Exception)
                {

                    MessageBox.Show("Bạn hãy chọn chữ ký số để có thể thực hiện ký!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

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
                signatureAppearance.Contact = DataStore.Instance.Customer.Email;
                signatureAppearance.Reason = reason;

                if (reason == "Ký biên bản lắp đặt" && pdfReader.NumberOfPages > 1)
                {
                    x = x + 110;
                    y = y + 850 - 700 + 50;

                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x - 100 - 15 - 20, y - 45 + 700 + 65 , x - 100 + 120, y - 45 + 700 + 165), pdfReader.NumberOfPages, "Signature");
                }
                else
                {
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x, y, x + 120, y + 120), pdfReader.NumberOfPages, "Signature");
                }

                signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);

                byte[] pdfBytes = File.ReadAllBytes(savePathsign.Replace("_sign.pdf", "_signed.pdf"));
                var stream = new MemoryStream(pdfBytes);
                PdfiumViewer.PdfDocument pdfDocument = PdfiumViewer.PdfDocument.Load(stream);
                pdfViewer.Document = pdfDocument;

                MessageBox.Show("Đã ký thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private async void OpenImageFileDialog(float x, float y, string reason)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn ảnh chữ ký";
                openFileDialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Xử lý ảnh đã chọn
                    string selectedFilePath = openFileDialog.FileName;
                    X509Store store = new X509Store(StoreLocation.CurrentUser);
                    store.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(store.Certificates, null, null, X509SelectionFlag.SingleSelection);
                    if (sel.Count > 0)
                    {
                        X509Certificate2 cert = new X509Certificate2();
                        try
                        {
                            cert = sel[0];
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Bạn hãy chọn chữ ký số để có thể thực hiện ký!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                        Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                        Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] {
                        cp.ReadCertificate(cert.RawData)};

                        IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");

                        string savePathsign = DataStore.Instance.SavePathSign;
                        PdfReader pdfReader = new PdfReader(savePathsign);

                        FileStream signedPdf = new FileStream(savePathsign.Replace("_sign.pdf", "_signed.pdf"), FileMode.Create);

                        PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
                        PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                        signatureAppearance.Reason = reason;
                        signatureAppearance.SignatureCreator = DataStore.Instance.Customer.FullName;
                        signatureAppearance.Contact = DataStore.Instance.Customer.Email;
                        signatureAppearance.SignDate = DateTime.Now;
                        signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(selectedFilePath);

                        if (reason == "Ký biên bản lắp đặt" && pdfReader.NumberOfPages > 1)
                        {
                            x = x + 110;
                            y = y + 850 - 700 + 50;

                            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x - 100 - 15 - 20, y - 45 + 700 + 55, x - 100 + 120, y - 45 + 700 + 155), pdfReader.NumberOfPages, "Signature");
                        }
                        else
                        {
                            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(x - 25, y - 35, x + 130, y + 100), pdfReader.NumberOfPages, "Signature");
                        }

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

                MessageBox.Show("Sau khi quý khách nhấn nút [Ok] Quá trình ký số diễn ra, vui lòng chờ cho đến khi có thông báo tiếp theo. Quá trình này có thể sẽ diễn ra trong vài phút.\nXin cảm ơn quý khách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                int isSuccess = 0;
                if (TypeDocument.SelectedItem.ToString() == "Hợp đồng")
                {
                    DoneContract doneContract = new DoneContract()
                    {
                        DConTractName = DataStore.Instance.PendingContract.PContractName,
                        PContractID = int.Parse(DataStore.Instance.PendingContract.PContractID),
                        Base64File = base64String + "*" + DataStore.Instance.Token,
                    };

                    isSuccess = await sendDContractRepository.PostContract(doneContract);
                }
                else
                {
                    DoneMinute doneMinute = new DoneMinute()
                    {
                        DMinuteName = DataStore.Instance.PendingMinute.MinuteName,
                        PMinuteID = DataStore.Instance.PendingMinute.PendingMinuteId,
                        Base64File = base64String + "*" + DataStore.Instance.Token,
                    };

                    isSuccess = await sendDMinuteRepository.PostDMinute(doneMinute);
                }
                if (isSuccess != 0)
                {
                    MessageBox.Show("Đã hoàn tất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đã có lỗi xảy ra.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pdfViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
