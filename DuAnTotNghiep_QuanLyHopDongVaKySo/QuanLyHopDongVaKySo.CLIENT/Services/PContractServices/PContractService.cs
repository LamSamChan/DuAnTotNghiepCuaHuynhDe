using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PContractServices
{
    public class PContractService : IPContractService
    {
        private readonly HttpClient _httpClient;
        public PContractService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> addAsnyc(PostPendingContract PContract)
        {
            string json = JsonConvert.SerializeObject(PContract);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PostAsync("api/PContract/AddPContractAsnyc", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return PContract.CustomerId.ToString();
                }
                return null;
            }
        }

        public async Task<List<PendingContract>> getAllAsnyc()
        {

            var response = await _httpClient.GetFromJsonAsync<List<PendingContract>>("api/PContract");
            return response;
    }
        public async Task<PendingContract> getByIdAsnyc(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<PendingContract>($"api/PContract/{id}");
            
            return response;
        }

        public async Task<string> updateAsnyc(PutPendingContract PContract)
        {
            string json = JsonConvert.SerializeObject(PContract);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _httpClient.PutAsync("api/PContract/Update", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return PContract.CustomerId.ToString();
                }
                return null;
            }
        }
    }
}
