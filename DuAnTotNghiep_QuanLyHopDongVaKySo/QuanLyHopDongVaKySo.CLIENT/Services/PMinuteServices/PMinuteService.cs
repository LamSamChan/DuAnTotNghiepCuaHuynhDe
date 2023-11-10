using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices
{
    public class PMinuteService : IPMinuteService
    {
        private readonly HttpClient _httpClient;
        public PMinuteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<PendingMinute>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PendingMinute>>("api/PMinute");
            return response;
        }

        public async Task<PendingMinute> GetById(int pMinuteId)
        {
            var response = await _httpClient.GetFromJsonAsync<PendingMinute>($"api/PMinute/{pMinuteId}");
            return response;
        }

        public async Task<int> GetTaskFormIRequirement(PostGetTaskFromIR task)
        {

            string json = JsonConvert.SerializeObject(task);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/PMinute", content))
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
