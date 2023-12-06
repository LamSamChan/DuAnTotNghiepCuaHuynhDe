using QuanLyHopDongVaKySo.SigningWithUsbToken.InstanceData;
using QuanLyHopDongVaKySo.SigningWithUsbToken.Repository;
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
    public partial class VerifyCustomerView : Form
    {
        private VerifyRepository verify = new VerifyRepository();
        private CustomerRepository customer = new CustomerRepository();

        public VerifyCustomerView()
        {
            InitializeComponent();
        }

        private async void verifyButton_Click(object sender, EventArgs e)
        {
            try
            {
                string token = null;
                if (!String.IsNullOrEmpty(identification.Text))
                {
                    token = await verify.VerifyCustomer(identification.Text);
                    if (token != null)
                    {
                        DataStore.Instance.Token = token;
                        DataStore.Instance.Customer = await customer.GetCustomer(identification.Text);

                        MainView mainView = new MainView();
                        this.Hide();
                        mainView.Closed += (s, args) => this.Close();
                        mainView.Show();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Hãy điền thông tin xác thực !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Không tìm thấy khách hàng !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
