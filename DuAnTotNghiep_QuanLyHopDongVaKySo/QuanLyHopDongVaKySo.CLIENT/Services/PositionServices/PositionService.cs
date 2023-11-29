using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PositionServices
{
    public class PositionService : IPositionService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public PositionService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
        public async Task<int> AddPositionAsync(Position position)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/Positions/AddNew",content);
                if(reponse.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<Position>> GetAllNotHidden()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<Position>>("api/Positions/NotHidden");
            return reponse;
        }

        public async Task<List<Position>> GetAllPositionsAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<List<Position>>("api/Positions");
            return reponse;
        }

        public async Task<Position> GetPositionByIdAsync(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var reponse = await _httpClient.GetFromJsonAsync<Position>($"api/Positions/{id}");
            return reponse;
        }

        public async Task<int> UpdatePositionAsync(Position position)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/Positions/Update", content);
                if (reponse.IsSuccessStatusCode)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
