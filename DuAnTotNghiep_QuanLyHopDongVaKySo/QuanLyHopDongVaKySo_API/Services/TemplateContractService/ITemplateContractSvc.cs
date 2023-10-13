using QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo_API.Services.TemplateContractService
{
    public interface ITemplateContractSvc
    {
        Task<List<TemplateContract>> getlist();
        Task<TemplateContract> getById(int id);
        Task<int> addTempContract(TemplateContract templateContract);
        Task<int> updateTempContract(TemplateContract templateContract);
        Task<bool> deleteTempContract(int id);
    }
}
