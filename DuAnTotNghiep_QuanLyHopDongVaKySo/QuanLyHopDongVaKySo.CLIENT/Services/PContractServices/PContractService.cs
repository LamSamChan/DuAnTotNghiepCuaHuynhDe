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
            using (var response = await _httpClient.PostAsync("api/PContract", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
        }

        public async Task<List<PContractViewModel>> getAllAsnyc()
        {

            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>("api/PContract");
            return response;
    }
        public async Task<PContractViewModel> getByIdAsnyc(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<PContractViewModel>($"api/PContract/{id}");
            
            return response;
        }
        public async Task<List<PContractViewModel>> getListEmpId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListCusId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetByCusId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCustomerSigns()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/WaitCustomerSigns");

            return response;
        }

        public async Task<List<PContractViewModel>> getListWaitDirectorSigns()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/WaitDirectorSigns");

            return response;
        }
        public async Task<List<PContractViewModel>> getListRefuse()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetRefuse");

            return response;
        }
        public async Task<List<PContractViewModel>> getListRefuseByEmpId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetRefuseByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCusSignsByDirId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitCusSignsByDirId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCusSignsByEmpId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitCusSignsByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitDirSignsEmpId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitDirSignsEmpId/{id}");

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
