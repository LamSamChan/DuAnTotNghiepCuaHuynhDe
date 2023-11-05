using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
namespace QuanLyHopDongVaKySo_API.Services.DoneContractService
{
    public interface IDoneContractSvc
    {
        Task<List<DoneContract>> getAllAsnyc(); 
        Task<DoneContract> getByIdAsnyc(int id);
        Task<DoneContract> addAsnyc(PutPendingContract Pcontract);
        Task<string> updateAsnyc(PutDContract dContract);

        Task<List<DContractViewModel>> getListIsEffect();
        

        
    }
}
