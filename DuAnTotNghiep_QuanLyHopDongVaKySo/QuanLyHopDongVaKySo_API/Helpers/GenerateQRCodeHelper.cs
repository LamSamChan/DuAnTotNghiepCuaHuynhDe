using iTextSharp.text.pdf.qrcode;
using QRCoder;
using QuanLyHopDongVaKySo_API.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IGenerateQRCodeHelper
    {
        byte[] GenerateQRCode(string text, int pContractID);
        byte[] BitmapToByteArray(Bitmap bitmap);
    }
    public class GenerateQRCodeHelper : IGenerateQRCodeHelper
    {
        public byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public byte[] GenerateQRCode(string text, int pContractID)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text,
            QRCodeGenerator.ECCLevel.Q);

            string qrrPath = $"AppData/PContracts/{pContractID}/" + pContractID + ".qrr";
            qrCodeData.SaveRawData(qrrPath, QRCodeData.Compression.Uncompressed);

            QRCodeData qrCodeData1 = new QRCodeData(qrrPath, QRCodeData.Compression.Uncompressed);

            QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData1);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            string pngPath = null;
            using (MemoryStream memoryStream = new MemoryStream(BitmapToByteArray(qrCodeImage)))
            {
                pngPath = $"AppData/PContracts/{pContractID}/" + pContractID + ".png";
                Image.FromStream(memoryStream).Save(pngPath);
            }

            FileStream fs = new FileStream(qrrPath, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(qrrPath);

            byte[] imageBytes = File.ReadAllBytes(pngPath);
            return imageBytes;
        }
    }
}
