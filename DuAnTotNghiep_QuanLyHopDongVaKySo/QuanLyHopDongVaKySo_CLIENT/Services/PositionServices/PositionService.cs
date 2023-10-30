using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo_CLIENT.Services.PositionServices
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
                    var ps = await reponse.Content.ReadAsStringAsync();
                    return int.Parse(ps);
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

        public async Task<IEnumerable<Position>> GetAllPositionsAsync()
        {
            var reponse = await _httpClient.GetFromJsonAsync<IEnumerable<Position>>("api/Position");
            return reponse;
        }

        public async Task<Position> GetPositionByIdAsync(int id)
        {
            var reponse = await _httpClient.GetFromJsonAsync<Position>($"api/Position/{id}");
            return reponse;
        }

        public async Task<int> UpdatePositionAsync(Position position)
        {
            var content = new StringContent(JsonConvert.SerializeObject(position), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/Position/Update", content);
                if (reponse.IsSuccessStatusCode)
                {
                    var ps = await reponse.Content.ReadAsStringAsync();
                    return int.Parse(ps);
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
