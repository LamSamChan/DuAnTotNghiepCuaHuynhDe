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

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    public partial class MainView : Form
    {
        private ContractRepository contractRepository = new ContractRepository();
        private MinuteRepository minuteRepository = new MinuteRepository();

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
                    int contractId = Convert.ToInt32(inputContractId.Text);
                    var pContract = await contractRepository.GetPContractById(contractId);
                    if (pContract != null)
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
                        string savePath = Path.Combine(Application.StartupPath, "AppData", $"{pMinute.MinuteName}.pdf");
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
    }
}
