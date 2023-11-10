using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
namespace QuanLyHopDongVaKySo_API.Services.DoneContractService
{
    public interface IDoneContractSvc
    {
        
        Task<DoneContract> getByIdAsnyc(string id);
        Task<DoneContract> addAsnyc(PutPendingContract Pcontract);
        Task<string> updateAsnyc(PutDContract dContract);
        Task<List<DoneContract>> GetAll();
        Task<DContractViewModel> getByIView(int id);
        Task<List<DContractViewModel>> getAllAsnyc();
        Task<List<DContractViewModel>> getListIsEffect();
        Task<List<DContractViewModel>> getListByCusId(string id);
        Task<List<DContractViewModel>> getListByEmpId(string id);
        Task<List<DContractViewModel>> getListByDirectorId(string id);


    }
}
