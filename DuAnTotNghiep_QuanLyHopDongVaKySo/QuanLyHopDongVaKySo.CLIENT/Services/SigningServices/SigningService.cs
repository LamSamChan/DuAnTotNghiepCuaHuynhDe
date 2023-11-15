using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.SigningServices
{
    public class SigningService : ISigningService
    {
        private readonly HttpClient _httpClient;
        public SigningService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> SignContractByCustomer(SigningModel signing)
        {
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
