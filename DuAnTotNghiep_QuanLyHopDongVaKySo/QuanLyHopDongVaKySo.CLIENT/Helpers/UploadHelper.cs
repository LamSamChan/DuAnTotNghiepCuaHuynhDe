﻿using Microsoft.Extensions.Primitives;

namespace QuanLyHopDongVaKySo.CLIENT.Helpers
{
    public interface IUploadHelper
    {
        string UploadImage(IFormFile file, string rootPath, string category);
        string UploadPDF(IFormFile file, string rootPath, string category);
        void RemoveImage(string filePath);
        IFormFile ConvertBase64ToIFormFile(string base64String, string fileName, string contentType);


    }
    public class UploadHelper : IUploadHelper
    {
        public string UploadImage(IFormFile file, string rootPath, string category)
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            string dirPath = rootPath + @"\" + category;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = dirPath + @"\" + file.FileName;

            if (!File.Exists(filePath))
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return filePath;
        }

        public void RemoveImage(string filePath)
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

        public string UploadPDF(IFormFile file, string rootPath, string category)
        {
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }
            string dirPath = rootPath + @"\" + category;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = dirPath + @"\" + file.FileName;

            if (!File.Exists(filePath))
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return filePath;
        }
    }
}
