using Ghostscript.NET.Rasterizer;
using iTextSharp.text.pdf;
using Spire.Pdf.Graphics;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo.CLIENT.Helpers
{
    public interface IPdfToImageHelper
    {
        List<string> PdfToPng(string inputFile, int idContract, string typeDoc);
    }
    public class PdfToImageHelper : IPdfToImageHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PdfToImageHelper(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public List<string> PdfToPng(string inputFile, int idContract, string typeDoc)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(inputFile, FileMode.Open);
                // Thực hiện các thao tác trên fs ở đây
            }
            catch (IOException ex)
            {
                fs?.Close();
            }
            finally
            {
                // Đảm bảo rằng tệp tin được đóng dù có lỗi hay không
                fs?.Close();
            }

            int totalPage;

            PdfReader pdfReader = new PdfReader(inputFile);
            totalPage = pdfReader.NumberOfPages;

            FileStream fs1 = null;
            try
            {
                fs1 = new FileStream(inputFile, FileMode.Open);
                // Thực hiện các thao tác trên fs ở đây
            }
            catch (IOException ex)
            {
                fs1?.Close();
            }
            finally
            {
                // Đảm bảo rằng tệp tin được đóng dù có lỗi hay không
                fs1?.Close();
            }

            List<string> output = new List<string>();
            string outputDirectoryPath = null;
            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {

                //locallhost
                rasterizer.Open(inputFile); //opens the PDF file for rasterizing
                if (typeDoc == "minute")
                {
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath,$"MinuteImage/{idContract}");
                }
                else if (typeDoc == "contract")
                {
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, $"ContractImage/{idContract}");
                }
                else if (typeDoc == "tcontract")
                {
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, $"TContractImage/{idContract}");
                }
                else
                {
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath, $"TMinuteImage/{idContract}");
                }

                //server

                if (!Directory.Exists(outputDirectoryPath))
                {
                    Directory.CreateDirectory(outputDirectoryPath);
                }
                //set the output image(png's) complete path
                var outputImagePath = outputDirectoryPath + @"/";
                //converts the PDF pages to png's 
                for (int i = 0; i < totalPage; i++)
                {
                    var outputPNGPath = outputImagePath + $"{i + 1}.png";
                    output.Add(outputPNGPath);
                    var pdf2PNG = rasterizer.GetPage(300, i + 1);
                    //save the png's
                    pdf2PNG.Save(outputPNGPath, ImageFormat.Png);
                }
            }
            FileStream fs2 = null;
            try
            {
                fs2 = new FileStream(inputFile, FileMode.Open);
                // Thực hiện các thao tác trên fs ở đây
            }
            catch (IOException ex)
            {
                fs2?.Close();
            }
            finally
            {
                // Đảm bảo rằng tệp tin được đóng dù có lỗi hay không
                fs2?.Close();
            }
            return output;
        }
    }
}
