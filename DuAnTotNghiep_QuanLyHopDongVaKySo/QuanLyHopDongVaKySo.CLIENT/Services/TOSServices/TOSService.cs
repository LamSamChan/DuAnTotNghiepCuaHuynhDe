using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace QuanLyHopDongVaKySo.CLIENT.Services.TOSServices
{
    public class TOSService : ITOSService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public TOSService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<int> AddNew(PostTOS typeOfService)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string json = JsonConvert.SerializeObject(typeOfService);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/TypeOfServices/AddNew", content))
                {

                    if (reponse.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<TypeOfService>> GetAll()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<TypeOfService>>("api/TypeOfServices");
            return reponse;
        }

        public async Task<TypeOfService> GetById(int typeOfService_ID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<TypeOfService>($"api/TypeOfServices/{typeOfService_ID}");
            return reponse;
        }

        public async Task<PutTOS> GetByPutId(int typeOfService_ID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<TypeOfService>($"api/TypeOfServices/{typeOfService_ID}");
            PutTOS put = new PutTOS() { 
                TOS_ID = reponse.TOS_ID,
                ServiceName = reponse.ServiceName,
                Price = reponse.Price,
                PerTime = reponse.PerTime,
                isHidden = reponse.isHidden,
                TContractID = reponse.templateContractID,
                TMinuteID = reponse.templateMinuteID
            };
            return put;
        }

        public async Task<int> Update(PutTOS typeOfService)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            string json = JsonConvert.SerializeObject(typeOfService);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync("api/TypeOfServices/Update", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return typeOfService.TOS_ID;
                }
                return 0;
            }
        }
    }
}
