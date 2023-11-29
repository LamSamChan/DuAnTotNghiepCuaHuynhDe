using System;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public interface IPendingContractSvc
    {
        
        Task<PendingContract> getByIdAsnyc(int id);
        Task<PendingContract> getByIdForWinformAsnyc(int id, string cusId);

        Task<string> addAsnyc(PostPendingContract PContract);
        Task<string> updateAsnyc(PutPendingContract PContract);
        Task<bool> deleteAsnyc(int id);
        Task<int> updatePContractFile(int id, string File, string base64File);
        Task<ContractInternet> ExportContract(PendingContract PContract, Employee? employee);

        // view model
        Task<PContractViewModel> geByIdView(int id);
        Task<List<PContractViewModel>> getAllAsnyc();
        Task<List<PContractViewModel>> getListWaitDirectorSigns();
        Task<List<PContractViewModel>> getListWaitCustomerSigns();
        Task<List<PContractViewModel>> getListEmpId(string id);
        Task<List<PContractViewModel>> getListCusId(string id);
        Task<List<PContractViewModel>> getListRefuse();
        Task<List<PContractViewModel>> getListRefuseByEmpId(string id);
        Task<List<PContractViewModel>> getListWaitCusSignsByEmpId(string id);
        Task<List<PContractViewModel>> getListWaitCusSignsByDirId(string id);
        Task<List<PContractViewModel>> getListWaitDirSignsByEmpId(string id);
        Task<List<PContractViewModel>> getListDirSignsByEmpId(string id);

    }
}
