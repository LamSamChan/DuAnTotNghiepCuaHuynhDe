using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services
{
   public interface IAuthServices
    {
        Task<int> Login(VMLogin login);
    }
    public class AuthServices : IAuthServices
    {
        private readonly HttpClient _httpClient;
        public AuthServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> Login(VMLogin login)
        {
            string json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Auth", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}
