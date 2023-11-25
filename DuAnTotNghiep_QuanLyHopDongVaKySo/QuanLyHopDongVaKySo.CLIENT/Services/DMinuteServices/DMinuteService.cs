using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices
{
    public class DMinuteService : IDMinuteService
    {
        private readonly HttpClient _httpClient;
        public DMinuteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoneMinute>> GetListByEmpId(string EmployeeId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DoneMinute>>($"api/DMinute/Employee/{EmployeeId}");
            return response;
        }

        public async Task<List<DoneMinute>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<DoneMinute>>("api/DMinute");
            return response;
        }

        public async Task<DoneMinute> GetById(int dMinuteId)
        {
            var response = await _httpClient.GetFromJsonAsync<DoneMinute>($"api/DMinute/{dMinuteId}");
            return response;
        }
    }
}
