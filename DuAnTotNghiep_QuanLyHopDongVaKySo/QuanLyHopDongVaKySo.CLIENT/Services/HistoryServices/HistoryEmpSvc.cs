using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices
{
    public class HistoryEmpSvc : IHistoryEmpSvc
    {

        private readonly HttpClient _httpClient;
        public HistoryEmpSvc(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int> AddNew(OperationHistoryEmp oHistoryEmp)
        {
            string json = JsonConvert.SerializeObject(oHistoryEmp);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                using (var reponse = await _httpClient.PostAsync("api/HistoryEmp/AddNew", content))
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

        public async Task<List<OperationHistoryEmp>> GetAll()
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<OperationHistoryEmp>>("api/HistoryEmp");
            return reponse;
        }

        public async Task<List<OperationHistoryEmp>> GetListById(string emp_ID)
        {
            var reponse = await _httpClient.GetFromJsonAsync<List<OperationHistoryEmp>>($"api/HistoryCus/{emp_ID}");
            return reponse;
        }
    }
}
