using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
namespace QuanLyHopDongVaKySo.CLIENT.Services.TContractServices
{
    public interface ITContractService
    {
        Task<List<TemplateContract>> getAllAsnyc();
        Task<TemplateContract> getByIdAsnyc(int id);
        Task<int> addAsnyc(PostTContract tContract);
        Task<int> updateAsnyc(PutTContract tContract);
        Task<int> DeleteTContract(int id);
    }
}
