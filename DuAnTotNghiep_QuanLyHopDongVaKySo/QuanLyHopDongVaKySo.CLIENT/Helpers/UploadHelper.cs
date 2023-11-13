namespace QuanLyHopDongVaKySo.CLIENT.Helpers
{
    public interface IUploadHelper
    {
        string UploadImage(IFormFile file, string rootPath, string category);
        void RemoveImage(string filePath);

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


    }
}
