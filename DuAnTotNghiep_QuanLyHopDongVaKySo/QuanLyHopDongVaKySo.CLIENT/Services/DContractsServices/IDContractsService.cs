using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
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
        Task<string> SignContractWithUSBToken(PostDContract_Usb dContract);
        Task<List<DContractViewModel>> getListIsEffect();
        Task<List<DContractViewModel>> getListByCusId(string id);
        Task<List<DContractViewModel>> getListByEmpId(string id);
        Task<List<DContractViewModel>> getListByDirectorId(string id);
        Task<List<DoneContract>> getAll();

    }
}
