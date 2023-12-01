using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumViewer;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Repository;
using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Helpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Org.BouncyCastle.Crypto.Tls;
using System.Diagnostics;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    public partial class MainView : Form
    {
        private ContractRepository contractRepository = new ContractRepository();
        private MinuteRepository minuteRepository = new MinuteRepository();
        private Signing signing = new Signing();
        private string savePath = null;
        // private UploadFileHelper uploadFile = new UploadFileHelper();

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
                        byte[] pdfBytes = Convert.FromBase64String(pContract.Base64File);
                        string savePath = Path.Combine(Application.StartupPath, "AppData", $"{pContract.PContractName}.pdf");
                        SaveByteArrayToFile(savePath, pdfBytes);

                        var stream = new MemoryStream(pdfBytes);
                        PdfDocument pdfDocument = PdfDocument.Load(stream);
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
                        savePath = Path.Combine(Application.StartupPath, "AppData", $"{pMinute.MinuteName}.pdf");
                        SaveByteArrayToFile(savePath, pdfBytes);

                        var stream = new MemoryStream(pdfBytes);
                        PdfDocument pdfDocument = PdfDocument.Load(stream);
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

        private void signContract_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại cho phép chọn giữa hai options
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn ký bằng chữ chữ ký không?", "Lựa chọn kiểu chữ ký", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes) // Option A
            {
                OpenImageFileDialog();
            }
            else // Option B
            {

            }
        }

        private string serial = null;
        private void OpenImageFileDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Chọn ảnh chữ ký";
                openFileDialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Xử lý ảnh đã chọn
                    string selectedFilePath = openFileDialog.FileName;
                    if (serial == null)
                    {
                        serial = signing.GetSerialNumber();
                    }

                    if (serial != null)
                    {
                        if (String.IsNullOrEmpty(tokenPassword.Text))
                        {
                            MessageBox.Show("Bạn hãy điền mật khẩu token để thực hiện ký!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            var cert = signing.loadX509byPass(serial, tokenPassword.Text);

                            string fontPath = @"../../../Font/texgyretermes-regular.otf";

                            Certicate myCert = new Certicate(cert);

                            string outputPath = savePath.Replace(".pdf", "_signed.pdf");

                            PDFSigner pdfs = new PDFSigner(fontPath, savePath, outputPath, myCert);
                            var rectangle = new iTextSharp.text.Rectangle((int)14, (int)757, (int)71, (int)778);
                            pdfs.SignImageV2(openFileDialog.FileName, rectangle, "Signature", false);
                            MessageBox.Show("Đã ký xong, file kết quả là " + outputPath + "!");

                            // open in default PDF application
                            Process process = Process.Start(outputPath);
                            process.WaitForExit();

                            byte[] pdfBytes = File.ReadAllBytes(outputPath);
                            var stream = new MemoryStream(pdfBytes);
                            PdfDocument pdfDocument = PdfDocument.Load(stream);
                            pdfViewer.Document = pdfDocument;

                        }
                    }
                    else
                    {
                        MessageBox.Show("Bạn cần chọn Certificate!");
                    }


                }
            }
        }
    }
}
