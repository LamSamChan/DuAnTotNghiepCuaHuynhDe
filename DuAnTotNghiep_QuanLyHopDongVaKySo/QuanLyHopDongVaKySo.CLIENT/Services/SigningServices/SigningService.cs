using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.SigningServices
{
    public class SigningService : ISigningService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string token;

        public SigningService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }
        public string Token
        {
            get
            {
                if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("token")))
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString("token");

                }
                else
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString(SessionKey.Customer.CustomerToken);
                }
                return token;
            }
            set { this.token = value; }
        }
        public async Task<string> SignContractByCustomer(SigningModel signing)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            string json = JsonConvert.SerializeObject(signing);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Signing/CustomerSignContract", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else return null;
               
            }
        }

        public async Task<string> SignContractByDirector(SigningModel signing)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            string json = JsonConvert.SerializeObject(signing);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Signing/DirectorSignContract", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else return null;

            }
        }

        public async Task<string> SignMinuteByCustomer(SigningModel signing)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            string json = JsonConvert.SerializeObject(signing);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Signing/CustomerSignMinute", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else return null;

            }
        }

        public async Task<string> SignMinuteByInstaller(SigningModel signing)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(2);
            string json = JsonConvert.SerializeObject(signing);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/Signing/InstallerSignMinute", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else return null;

            }
        }
    }
}
