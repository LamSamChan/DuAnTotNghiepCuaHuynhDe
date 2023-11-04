using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PContractServices
{
    public interface IPContractService
    {
        Task<List<PendingContract>> getAllAsnyc();
        Task<PendingContract> getByIdAsnyc(int id);
        Task<string> addAsnyc(PostPendingContract PContract);
        Task<string> updateAsnyc(PutPendingContract PContract);
        Task<bool> deleteAsnyc(int id);
        Task<int> updatePContractFile(int id, string File);
        Task<ContractInternet> ExportContract(PendingContract PContract);
        Task<List<DContractViewModel>> getListEffect();
    }
}
