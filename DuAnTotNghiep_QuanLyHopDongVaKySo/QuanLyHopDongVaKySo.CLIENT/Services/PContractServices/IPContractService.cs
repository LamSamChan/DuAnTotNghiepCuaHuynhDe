using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.PContractServices
{
    public interface IPContractService
    {
        Task<List<PContractViewModel>> getAllAsnyc();
        Task<PContractViewModel> getByIdAsnyc(string id);
        Task<string> addAsnyc(PostPendingContract PContract);
        Task<string> updateAsnyc(PutPendingContract PContract);

        Task<List<PContractViewModel>> getListWaitDirectorSigns();
        Task<List<PContractViewModel>> getListWaitCustomerSigns();
        Task<List<PContractViewModel>> getListEmpId(string id);
        Task<List<PContractViewModel>> getListCusId(string id);
        Task<List<PContractViewModel>> getListRefuseByEmpId(string id);
        Task<List<PContractViewModel>> getListRefuse();
        Task<List<PContractViewModel>> getListWaitCusSignsByEmpId(string id);
        Task<List<PContractViewModel>> getListWaitCusSignsByDirId(string id);
        Task<List<PContractViewModel>> getListWaitDirSignsEmpId(string id);
        Task<List<PContractViewModel>> getListDirSignsEmpId(string id);
    }
}
