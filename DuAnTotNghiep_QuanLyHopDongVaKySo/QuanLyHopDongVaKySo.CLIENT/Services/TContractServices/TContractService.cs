using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TContractServices
{
    public class TContractService : ITContractService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public TContractService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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


        public async Task<int> addAsnyc(PostTContract tContract)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var json = JsonConvert.SerializeObject(tContract);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/TContract", content);
                if (reponse.IsSuccessStatusCode)
                {
                    int tContractID = 0;
                    bool result = Int32.TryParse(await reponse.Content.ReadAsStringAsync(), out tContractID);
                    if (result)
                    {
                        return tContractID;
                    }
                    else {
                        return 0;
                    }
                }
                else{ return 0; }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<TemplateContract>> getAllAsnyc()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            try
            {
                var reponse = await _httpClient.GetFromJsonAsync<List<TemplateContract>>("api/TContract");
                return reponse;
            }
            catch (Exception ex)
            {
                return new List<TemplateContract>();
            }
        }

        public async Task<TemplateContract> getByIdAsnyc(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<TemplateContract>($"api/TContract/{id}");
            return reponse;
        }

        public async Task<int> DeleteTContract(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.DeleteAsync($"api/TContract/Delete/{id}");
            if (reponse.IsSuccessStatusCode)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> updateAsnyc(PutTContract tContract)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonConvert.SerializeObject(tContract), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/TContract", content);
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
