using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PContractServices
{
    public class PContractService : IPContractService
    {
        private readonly HttpClient _httpClient;
        public PContractService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Task<string> addAsnyc(PostPendingContract PContract)
        {
            throw new NotImplementedException();
        }

        public Task<bool> deleteAsnyc(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ContractInternet> ExportContract(PendingContract PContract)
        {
            throw new NotImplementedException();
        }

        public Task<List<PendingContract>> getAllAsnyc()
        {
            throw new NotImplementedException();
        }

        public Task<PendingContract> getByIdAsnyc(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DContractViewModel>> getListEffect()
        {
            var response = await _httpClient.GetFromJsonAsync<List<DContractViewModel>>("api/PContract/getAllEffect");
            return response;
        }

        public Task<string> updateAsnyc(PutPendingContract PContract)
        {
            throw new NotImplementedException();
        }

        public Task<int> updatePContractFile(int id, string File)
        {
            throw new NotImplementedException();
        }
    }
}
