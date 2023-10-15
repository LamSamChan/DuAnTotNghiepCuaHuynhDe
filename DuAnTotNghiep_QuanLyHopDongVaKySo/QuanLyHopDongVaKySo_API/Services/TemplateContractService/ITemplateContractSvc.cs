using QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo_API.Services.TemplateContractService
{
    public interface ITemplateContractSvc
    {
        Task<List<TemplateContract>> getTContractsAsnyc();
        Task<TemplateContract> getTContractAsnyc(int id);
        Task<int> addTContract(PostTContract tContract);
        Task<int> updateTContract (PutTContract tContract);
        Task<bool> deleteTContract (int id);
    }
}
