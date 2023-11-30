using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Esign
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string serial = "";
        private void button1_Click(object sender, EventArgs e)
        {
            Kyso kyso = new Esign.Kyso();

            if (serial == "")
            {
                serial =kyso.GetSerialNumber();
            }

            if(serial!="")
            {
                if(textBox1.Text=="")
                {
                    MessageBox.Show("Bạn cần nhập mật khẩu!");
                }
                else
                {
                    string directory = AppDomain.CurrentDomain.BaseDirectory;
                    
                    var cert = kyso.loadX509byPass(serial, textBox1.Text);
                    string fontPath = @"D:\HolligateSignatureDemo.ttf";
                    //cert = loadX509byPass();

                    //cert= 
                    Certicate myCert = new Certicate(cert);

                    //Certicate myCert = new Certicate(@"E:\MyCert.pfx", "MyPassword");
                    //myCert.pr
                    //myCert.pr
                    string outputFile = @"D:\Test_A4_" + "daky.pdf";
                    for(int i=1;i<1000;i++)
                    {
                        outputFile = @"D:\Test_A4_" + i+"daky.pdf";
                        if(!File.Exists(outputFile))
                        {
                            break;
                        }    
                    }

                    PDFSigner pdfs = new PDFSigner(fontPath, @"D:\LAMSAMCHAN_CV - Copy.pdf", outputFile, myCert);
                    string inputImg = @"D:\1668246155408.jpg";
                    var rectangle = new iTextSharp.text.Rectangle((int)14, (int)757, (int)71, (int)778);
                    pdfs.SignImageV2("Ký thử", "", "", inputImg, rectangle, 1, "Lý do", false);
                    MessageBox.Show("Đã ký xong, file kết quả là "+ outputFile + "!");

                    // open in default PDF application
                    Process process = Process.Start(outputFile);
                    process.WaitForExit();


                    /*
                    WebClient myWebClient = new WebClient();

                    string fileName = @"E:\PDF\Test_A4.pdf";
                    string uriString = "http://localhost:54826/Kyso/Testupload.aspx";
                    Console.WriteLine("Uploading {0} to {1} ...", fileName, uriString);


                    // Upload the file to the URI.
                    // The 'UploadFile(uriString,fileName)' method implicitly uses HTTP POST method.
                    byte[] responseArray = myWebClient.UploadFile(uriString, fileName);
                    */

                    /*
                    WebClient client = new WebClient();
                    client.DownloadFile("http://localhost:54826/UCT/DownLoadFile.aspx?ItemID=c7249b96-7b50-4d5a-a6bf-c1f5dbe3186a&thumuc=KysoD", @"E:\testing.pdf");
                    */
                    //var cert= GetCertificate();
                    //var cert= loadX509byPass();

                }
            }   
            else
            {
                MessageBox.Show("Bạn cần chọn Certificate!");
            }    
        }
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_Click(object sender,System.EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Exit")
            {
                //do something
            }

            if (e.ClickedItem.Text == "Open")
            {                
                if (FormWindowState.Minimized == WindowState)
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                }                    
            }
        }
    }
}
