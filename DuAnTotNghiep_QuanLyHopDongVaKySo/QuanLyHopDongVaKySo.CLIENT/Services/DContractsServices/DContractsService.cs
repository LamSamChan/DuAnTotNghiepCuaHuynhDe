using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Net.Http;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices
{
    public class DContractsService:IDContractsService
    {
        private readonly HttpClient _httpClient;
        public DContractsService(HttpClient httpClient) { 
            _httpClient = httpClient;
        }

        public async Task<List<DContractViewModel>> getAllAsnyc()
        {
            var response = await _httpClient.GetFromJsonAsync< List<DContractViewModel>>($"api/DContract/getAll");
            return response;
        }

        public async Task<DoneContract> getByIdAsnyc(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<DoneContract>($"api/DContract/getById/{id}");
            return response;
        }

        public async Task<List<DContractViewModel>> getListByCusId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DContractViewModel>>($"api/DContract/getByCustomerId/{id}");
            return response;
        }

        public async Task<List<DContractViewModel>> getListByDirectorId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DContractViewModel>>($"api/DContract/getByDirectorId/{id}");
            return response;
        }

        public async Task<List<DContractViewModel>> getListByEmpId(string id)
        {
            var response = await _httpClient.GetFromJsonAsync<List<DContractViewModel>>($"api/DContract/getByEmpId/{id}");
            return response;
        }

        public async Task<List<DContractViewModel>> getListIsEffect()
        {
            var response = await _httpClient.GetFromJsonAsync<List<DContractViewModel>>("api/DContract/getAllEffect");
            return response;
        }

        public async Task<string> updateAsnyc(PutDContract dContract)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dContract), Encoding.UTF8, "application/json");
            try
            {
                var reponse = await _httpClient.PutAsync("api/DContract/Update", content);
                if (reponse.IsSuccessStatusCode)
                {
                    return dContract.DContractID;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
