using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices
{
    public class TMinuteService : ITMinuteService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public TMinuteService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
                return token;
            }
            set { this.token = value; }
        }
        public async Task<int> AddNew(PostTMinute tMinute)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var json = JsonConvert.SerializeObject(tMinute);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/TMinute", content);
                if (reponse.IsSuccessStatusCode)
                {
                    int tMinuteID = 0;
                    bool result = Int32.TryParse(await reponse.Content.ReadAsStringAsync(), out tMinuteID);
                    if (result)
                    {
                        return tMinuteID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else { return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteTMinute(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.DeleteAsync($"api/TMinute/Delete/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<TemplateMinute>> GetAll()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<TemplateMinute>>("api/TMinute");
            return reponse;
        }

        public async Task<TemplateMinute> GetById(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<TemplateMinute>($"api/TMinute/{id}");
            return reponse;
        }

        public async Task<int> Update(PutTMinute tMinute)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonConvert.SerializeObject(tMinute), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/TMinute", content);
                if (reponse.IsSuccessStatusCode)
                {
                    return 1;
                }
                else { return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
