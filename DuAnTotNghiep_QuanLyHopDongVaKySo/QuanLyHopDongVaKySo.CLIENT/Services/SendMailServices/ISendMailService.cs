using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_CLIENT.Services.SendMailServices
{
    public interface ISendMailService
    {
        Task<string> SendMailAsync(SendMail sendMail);
    }
}
