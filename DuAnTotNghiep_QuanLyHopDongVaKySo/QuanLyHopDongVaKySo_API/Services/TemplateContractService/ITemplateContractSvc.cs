using QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo_API.Services.TemplateContractService
{
    public interface ITemplateContractSvc
    {
        Task<List<TemplateContract>> getAllAsnyc();
        Task<TemplateContract> getByIdAsnyc(int id);
        Task<int> addAsnyc(PostTContract tContract);
        Task<int> updateAsnyc (PutTContract tContract);
        Task<bool> deleteAsnyc (int id);
    }
}
