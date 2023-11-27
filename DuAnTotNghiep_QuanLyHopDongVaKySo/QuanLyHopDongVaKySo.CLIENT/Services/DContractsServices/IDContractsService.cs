using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices
{
    public interface IDContractsService
    {
        Task<List<DContractViewModel>> getAllView();
        Task<DContractViewModel> getByIdAsnyc(string id);
        Task<string> updateAsnyc(PutDContract dContract);
        Task<PutDContract> updateIsEffect(PutDContract dContract);
        Task<PutDContract> getByIdUnEffect(string id);
        Task<string> UnEffectContract(int id);

        Task<List<DContractViewModel>> getListIsEffect();
        Task<List<DContractViewModel>> getListByCusId(string id);
        Task<List<DContractViewModel>> getListByEmpId(string id);
        Task<List<DContractViewModel>> getListByDirectorId(string id);
        Task<List<DoneContract>> getAll();

    }
}
