using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Net.Http.Headers;
using System.Text;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PContractServices
{
    public class PContractService : IPContractService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private string token;

        public PContractService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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
                else
                {
                    token = _httpContextAccessor.HttpContext.Session.GetString(SessionKey.Customer.CustomerToken);
                }
                return token;
            }

            set { this.token = value; }
        }

        public async Task<string> addAsnyc(PostPendingContract PContract)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>("api/PContract");
            return response;
    }
        public async Task<PContractViewModel> getByIdAsnyc(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<PContractViewModel>($"api/PContract/{id}");
            
            return response;
        }
        public async Task<List<PContractViewModel>> getListEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListCusId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetByCusId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCustomerSigns()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/WaitCustomerSigns");

            return response;
        }

        public async Task<List<PContractViewModel>> getListWaitDirectorSigns()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/WaitDirectorSigns");

            return response;
        }
        public async Task<List<PContractViewModel>> getListRefuse()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetRefuse");

            return response;
        }
        public async Task<List<PContractViewModel>> getListRefuseByEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetRefuseByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCusSignsByDirId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitCusSignsByDirId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitCusSignsByEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitCusSignsByEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListWaitDirSignsEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetWaitDirSignsEmpId/{id}");

            return response;
        }
        public async Task<List<PContractViewModel>> getListDirSignsEmpId(string id)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            var response = await _httpClient.GetFromJsonAsync<List<PContractViewModel>>($"api/PContract/GetDirSignsEmpId/{id}");

            return response;
        }
        public async Task<string> updateAsnyc(PutPendingContract PContract)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
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
