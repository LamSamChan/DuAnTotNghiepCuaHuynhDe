using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.DoneContractService
{
    public interface IDoneContractSvc
    {
        Task<List<DoneContract>> getAllAsnyc(); 
        Task<DoneContract> getByIdAsnyc(int id);
        Task<DoneContract> addAsnyc(PutPendingContract Pcontract);
        Task<string> updateAsnyc(PutDContract dContract);
        
    }
}
