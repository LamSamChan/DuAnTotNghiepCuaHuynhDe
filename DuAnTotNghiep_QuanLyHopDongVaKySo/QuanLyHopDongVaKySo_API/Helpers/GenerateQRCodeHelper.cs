using QRCoder;
using QuanLyHopDongVaKySo_API.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace QuanLyHopDongVaKySo_API.Helpers
{
    public interface IGenerateQRCodeHelper
    {
        string GenerateQRCode(string text);
        byte[] BitmapToByteArray(Bitmap bitmap);
        IFormFile ConvertBase64ToIFormFile(string base64String, string fileName);
    }
    public class GenerateQRCodeHelper : IGenerateQRCodeHelper
    {
        public byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public string GenerateQRCode(string text)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = BitmapToByteArray(QrBitmap);
            string QrUri = Convert.ToBase64String(BitmapArray);
            return QrUri;
        }

        public IFormFile ConvertBase64ToIFormFile(string base64String, string fileName)
        {
            // Chuyển chuỗi Base64 thành byte[]
            byte[] bytes = Convert.FromBase64String(base64String);

            // Tạo một MemoryStream từ byte[]
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                // Tạo một IFormFile từ MemoryStream
                return new FormFile(ms, 0, ms.Length, "file", fileName+".png");
            }
        }
    }
}
