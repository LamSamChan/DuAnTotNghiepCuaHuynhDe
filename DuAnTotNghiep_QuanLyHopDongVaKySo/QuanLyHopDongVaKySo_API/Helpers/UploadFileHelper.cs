namespace QuanLyHopDongVaKySo_API.Helpers
{   
        public interface IUploadFileHelper
        {
            string UploadFile(IFormFile file, string rootPath, string category);
            void RemoveFile(string filePath);

        }
        public class UploadFileHelper : IUploadFileHelper
    {
            public string UploadFile(IFormFile file, string rootPath, string category)
            {
                //string path = Path.Combine(_hostingEnvironment.WebRootPath, "images", file.FileName);
                if (!Directory.Exists(rootPath))
                {
                    Directory.CreateDirectory(rootPath);
                }
                string dirPath = rootPath + @"/" + category;
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                string filePath = dirPath + @"/" + file.FileName;

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
        }
   
}
