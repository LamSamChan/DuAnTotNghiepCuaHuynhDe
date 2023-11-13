using Microsoft.Extensions.Primitives;

namespace QuanLyHopDongVaKySo_API.Helpers
{   
        public interface IUploadFileHelper
        {
            IFormFile ConvertBase64ToIFormFile(string base64String, string fileName, string contentType);
            string UploadFile(IFormFile file, string rootPath, string category, string extension);
            void RemoveFile(string filePath);

        }
        public class UploadFileHelper : IUploadFileHelper
    {
            public string UploadFile(IFormFile file, string rootPath, string category, string extension)
            {
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                string dirPath = rootPath + @"/" + category;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                string filePath = dirPath + @"/" + file.FileName +$"{extension}";

                if (!File.Exists(filePath))
                {
                    using (Stream stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return filePath;
            }

            public void RemoveFile(string filePath)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

        public IFormFile ConvertBase64ToIFormFile(string base64String, string fileName, string contentType)
        {
            // Giải mã chuỗi Base64 thành dãy byte
            byte[] bytes = Convert.FromBase64String(base64String);

            // Tạo một bộ đệm và ghi dãy byte vào đó
            var stream = new MemoryStream(bytes);

            // Tạo IFormFile với MemoryStream và đặt tên và kiểu nội dung tệp
            var formFile = new FormFile(stream, 0, bytes.Length, "file", fileName);
            var contentDisposition = "form-data; name=\"file\"; filename=\"" + fileName + "\"";
            formFile.Headers = new HeaderDictionary();
            formFile.Headers.Add("Content-Disposition", new StringValues(contentDisposition));
            formFile.Headers.Add("Content-Type", new StringValues(contentType));

            return formFile;
        }
    }
   
}
