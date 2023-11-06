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

        public async Task<List<PContractViewModel>> getAllAsnyc()
        {
            List<PContractViewModel> viewModels= new List<PContractViewModel>();
            var response = await _httpClient.GetFromJsonAsync<List<PendingContract>>("api/PContract");

            viewModels = response.Select(r => new PContractViewModel
            {
                PContractID = r.PContractID.ToString(),
                PContractName = r.PContractName,
                DateCreated = r.DateCreated.ToString("dd/MM/yyyy"),
                CustomerName = r.Customer != null ? r.Customer.FullName: " ",
                CustomerEmail = r.Customer != null ? r.Customer.Email : " ",
                IsDirector = r.IsDirector ? "Đã ký" : "Chưa ký",
                IsCustomer = r.IsCustomer ? "Đã ký" : "Chưa ký",
                IsRefuse = r.IsRefuse ? "Từ chối ký" : "Chờ ký",
                DirectorSignedId = r.DirectorSignedId.ToString(),
                EmployeeCreatedId = r.EmployeeCreatedId.ToString(),
                Reason = r.Reason,
                InstallationAddress = r.InstallationAddress,
                TOS_ID = r.TypeOfService != null ? r.TypeOfService.ServiceName : " ",
                PContractFile = r.PContractFile
            }).ToList();
            return viewModels;
    }
        public async Task<PContractViewModel> getByIdAsnyc(int id)
        {
            PContractViewModel viewModels = new PContractViewModel();
            var response = await _httpClient.GetFromJsonAsync<PendingContract>($"api/PContract/{id}");

            viewModels.PContractID = response.PContractID.ToString();
            viewModels.PContractName = response.PContractName;
            viewModels.DateCreated = response.DateCreated.ToString("dd/MM/yyyy");
            viewModels.CustomerName = response.Customer.FullName;
            viewModels.CustomerEmail = response.Customer.Email;
            viewModels.IsDirector = response.IsDirector ? "Đã ký" : "Chưa ký";
            viewModels.IsCustomer = response.IsCustomer ? "Đã ký" : "Chưa ký";
            viewModels.IsRefuse = response.IsRefuse ? "Từ chối ký" : "Chờ ký";
            viewModels.DirectorSignedId = response.DirectorSignedId.ToString();
            viewModels.EmployeeCreatedId = response.EmployeeCreatedId.ToString();
            viewModels.Reason = response.Reason;
            viewModels.InstallationAddress = response.InstallationAddress;
            viewModels.TOS_ID = response.TypeOfService.ServiceName;
            viewModels.PContractFile = response.PContractFile;
            
            return viewModels;
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
