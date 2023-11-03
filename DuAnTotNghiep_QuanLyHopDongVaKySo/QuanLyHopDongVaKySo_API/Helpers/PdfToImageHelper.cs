using Ghostscript.NET.Rasterizer;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IPdfToImageHelper
    {
        List<string> PdfToPng(string inputFile, int idPContract,string typeDoc);
    }
    public class PdfToImageHelper : IPdfToImageHelper
    {
        public List<string> PdfToPng(string inputFile, int idContract,string typeDoc)
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
                rasterizer.Open(inputFile); //opens the PDF file for rasterizing
                if (typeDoc == "minute")
                {
                    outputDirectoryPath = $"AppData/MinuteImage/{idContract}";
                }
                else
                {
                    outputDirectoryPath = $"AppData/ContractImage/{idContract}";
                }
                
                if(!Directory.Exists(outputDirectoryPath))
                {
                    Directory.CreateDirectory(outputDirectoryPath);
                }
                //set the output image(png's) complete path
                var outputImagePath = outputDirectoryPath + @"/";
                //converts the PDF pages to png's 
                for (int i = 0; i < totalPage; i++)
                {
                    var outputPNGPath = outputImagePath + $"{i+1}.png";
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
