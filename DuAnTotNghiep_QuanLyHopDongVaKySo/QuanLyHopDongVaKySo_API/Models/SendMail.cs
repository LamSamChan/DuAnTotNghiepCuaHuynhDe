namespace QuanLyHopDongVaKySo_API.Models
{
    public class SendMail
    {
        public string Subject { get; set; }
        public string? SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string? FromMail { get; set; }
        public string ToMail { get; set; }
        public string HtmlContent { get; set; }

    }
}
