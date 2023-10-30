using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IPdfToImageHelper
    {
        void PdfToPng(string inputFile, int idPContract, int totalPage);
    }
    public class PdfToImageHelper : IPdfToImageHelper
    {
        public void PdfToPng(string inputFile, int idContract, int totalPage)
        {

            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {
                rasterizer.Open(inputFile); //opens the PDF file for rasterizing
                var outputDirectoryPath = $"AppData/ContractImage/{idContract}";
                if(!Directory.Exists(outputDirectoryPath))
                {
                    Directory.CreateDirectory(outputDirectoryPath);
                }
                 //set the output image(png's) complete path
               

                //converts the PDF pages to png's 
                for (int i = 0; i < totalPage; i++)
                {
                    var outputPNGPath = Path.Combine(outputDirectoryPath, string.Format("{0}.png", i+1));
                    var pdf2PNG = rasterizer.GetPage(300, i + 1);
                    //save the png's
                    pdf2PNG.Save(outputPNGPath, ImageFormat.Png);
                }
            }
        }
    }
}
