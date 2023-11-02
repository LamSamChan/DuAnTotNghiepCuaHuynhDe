using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PositionServices
{
    public class PositionService : IPositionService
    {
        private readonly HttpClient _httpClient;
        public PositionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddPositionAsync(Position position)
        {
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PostAsync("api/Position/AddNew",content);
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
            var reponse = await _httpClient.GetFromJsonAsync<List<Position>>("api/Positions/NotHidden");
            return reponse;
        }

        public async Task<List<Position>> GetAllPositionsAsync()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<Position>>("api/Positions");
            return reponse;
        }

        public async Task<Position> GetPositionByIdAsync(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Position>($"api/Positions/{id}");
            return reponse;
        }

        public async Task<int> UpdatePositionAsync(Position position)
        {
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsJsonAsync("api/Positions/Update", content);
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
