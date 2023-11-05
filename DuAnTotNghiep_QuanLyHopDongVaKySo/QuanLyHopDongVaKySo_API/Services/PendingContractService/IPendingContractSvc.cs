using System;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public interface IPendingContractSvc
    {
        Task<List<PendingContract>> getAllAsnyc();
        Task<PendingContract> getByIdAsnyc(int id);
        Task<string> addAsnyc(PostPendingContract PContract);
        Task<string> updateAsnyc(PutPendingContract PContract);
        Task<bool> deleteAsnyc(int id);
        Task<int> updatePContractFile(int id, string File);
        Task<ContractInternet> ExportContract(PendingContract PContract, Employee? employee);
        Task<List<PContractViewModel>> getListWaitDirectorSigns();
        Task<List<PContractViewModel>> getListWaitCustomerSigns();
        Task<List<PContractViewModel>> getListIsRefuse();
    }
}
