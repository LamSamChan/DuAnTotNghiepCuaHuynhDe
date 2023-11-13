using Ghostscript.NET.Rasterizer;
using iTextSharp.text.pdf;
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
            FileStream fs1 = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            fs1.Close();
            int totalPage;

            PdfReader pdfReader = new PdfReader(inputFile);
            totalPage = pdfReader.NumberOfPages;
            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            fs.Close();
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

            return output;
        }
    }
}
