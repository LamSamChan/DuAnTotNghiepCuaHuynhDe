namespace QuanLyHopDongVaKySo_API.ViewModels
{
    public class SigningModel
    {
        public string Serial { get; set; }
        public string IdFile { get; set; }
        public string? Base64StringFile { get; set; }
        public string? ImagePath { get; set; }
        public string? Base64StringFileStamp { get; set; }
        public string? ImagePathStamp { get; set; }
    }
}
