using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using iTextSharp.text.pdf;
using Spire.Pdf.Graphics;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo.CLIENT.Helpers
{
    public interface IPdfToImageHelper
    {
        List<string> PdfToPng(string inputFile, int idContract, string typeDoc);
        void DeleteFileWithRetry(string filePath, int tryCount = 3);

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

            string ghostScriptVer = Path.Combine(_hostingEnvironment.WebRootPath, "Resource", "gsdll32.dll");
            //string ghostScriptVer = Path.Combine(_hostingEnvironment.WebRootPath, "Resource", "gsdll64.dll");

            GhostscriptVersionInfo gvi = new GhostscriptVersionInfo(ghostScriptVer);

            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {
                string categoryPath = null;
                //locallhost
                rasterizer.Open(inputFile, gvi,true); //opens the PDF file for rasterizing
                if (typeDoc == "minute")
                {
                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\MinuteImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }

                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath+ "\\MinuteImage", idContract.ToString());
                }
                else if (typeDoc == "contract")
                {
                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\ContractImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath+ "\\ContractImage", idContract.ToString());
                }
                else if (typeDoc == "pminute")
                {
                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\PMinuteImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\PMinuteImage", idContract.ToString());
                }
                else if (typeDoc == "pcontract")
                {
                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\PContractImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\PContractImage", idContract.ToString());
                }
                else if (typeDoc == "tcontract")
                {

                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\TContractImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath+ "\\TContractImage", idContract.ToString());
                }
                else
                {
                    categoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\TMinuteImage");
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }
                    outputDirectoryPath = Path.Combine(_hostingEnvironment.WebRootPath+ "\\TMinuteImage",idContract.ToString());
                }

                //server

                if (!Directory.Exists(outputDirectoryPath))
                {
                    Directory.CreateDirectory(outputDirectoryPath);
                }
                //set the output image(png's) complete path
                var outputImagePath = outputDirectoryPath + @"\";
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
        public void DeleteFileWithRetry(string filePath, int tryCount = 3)
        {
            for (int i = 0; i < tryCount; ++i)
            {
                try
                {
                    File.Delete(filePath);
                    break;
                }
                catch (IOException)
                {
                    if (i == tryCount - 1)
                        throw;
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }

}
