using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices
{
    public class PMinuteService : IPMinuteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public PMinuteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
                    token = _httpContextAccessor.HttpContext.Session.GetString(Constants.SessionKey.Customer.CustomerToken);
                }
                return token;
            }
            set { this.token = value; }
        }
        public async Task<List<PendingMinute>> GetAll()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PendingMinute>>("api/PMinute");
            return response;
        }

        public async Task<List<PendingMinute>> GetByEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PendingMinute>>($"api/PMinute/Employee/{id}");
            return response;
        }

        public async Task<PendingMinute> GetById(int pMinuteId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<PendingMinute>($"api/PMinute/{pMinuteId}");
            return response;
        }

        public async Task<string> GetTaskFormIRequirement(PostGetTaskFromIR task)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/PMinute", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
        }
    }
}
