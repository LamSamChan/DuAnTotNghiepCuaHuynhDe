using QuanLyHopDongVaKySo_API.Models;

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
