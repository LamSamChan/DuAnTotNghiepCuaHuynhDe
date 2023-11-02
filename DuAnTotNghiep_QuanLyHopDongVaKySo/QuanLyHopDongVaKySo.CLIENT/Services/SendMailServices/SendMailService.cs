using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.SendMailServices
{
    public class SendMailService : ISendMailService
    {
        private readonly HttpClient _httpClient;
        public SendMailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> SendMailAsync(SendMail sendMail)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SendMails/Send",sendMail);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
