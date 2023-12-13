using API = QuanLyHopDongVaKySo_API.Models;
using APIVM = QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
namespace QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices
{
    public interface IDContractsService
    {
        Task<List<APIVM.DContractViewModel>> getAllView();
        Task<APIVM.DContractViewModel> getByIdAsnyc(string id);
        Task<string> updateAsnyc(API.PutDContract dContract);
        Task<API.PutDContract> updateIsEffect(API.PutDContract dContract);
        Task<API.PutDContract> getByIdUnEffect(string id);
        Task<string> UnEffectContract(int id);
        Task<string> SignContractWithUSBToken(PostDContract_Usb dContract);
        Task<List<APIVM.DContractViewModel>> getListIsEffect();
        Task<List<APIVM.DContractViewModel>> getListByCusId(string id);
        Task<List<APIVM.DContractViewModel>> getListByEmpId(string id);
        Task<List<APIVM.DContractViewModel>> getListByDirectorId(string id);
        Task<List<API.DoneContract>> getAll();

        Task<List<API.DoneContract>> getAllNotInstallYet();


    }
}
