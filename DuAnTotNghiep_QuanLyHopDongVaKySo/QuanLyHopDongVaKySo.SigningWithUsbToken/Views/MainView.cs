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

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Views
{
    public partial class MainView : Form
    {
        private ContractRepository contractRepository = new ContractRepository();
       // private UploadFileHelper uploadFile = new UploadFileHelper();

        public MainView()
        {
            InitializeComponent();
        }

        private async void getContractButton_Click(object sender, EventArgs e)
        {
            try
            {

                int contractId = Convert.ToInt32(inputContractId.Text);
                var pContract = await contractRepository.GetPContractById(contractId);
                if (pContract != null)
                {
                    byte[] pdfBytes = Convert.FromBase64String(pContract.Base64File);
                    string savePath = Path.Combine(Application.StartupPath, "AppData", $"{pContract.PContractName}.pdf");
                    SaveByteArrayToFile(savePath, pdfBytes);

                }
                else
                {
                    MessageBox.Show("Không tìm thấy hợp đồng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
