using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Data;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices
{
    public class HistoryCusSvc : IHistoryCusSvc
    {
        private readonly HttpClient _httpClient;
        public HistoryCusSvc(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNew(OperationHistoryCus oHistoryCus)
        {
            string json = JsonConvert.SerializeObject(oHistoryCus);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/HistoryCus/AddNew", content))
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

        public async Task<List<OperationHistoryCus>> GetAll()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<OperationHistoryCus>>("api/HistoryCus");
            return reponse;
        }

        public async Task<List<OperationHistoryCus>> GetListById(string customer_ID)
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<OperationHistoryCus>>($"api/HistoryCus/{customer_ID}");
            return reponse;
        }
    }
}
