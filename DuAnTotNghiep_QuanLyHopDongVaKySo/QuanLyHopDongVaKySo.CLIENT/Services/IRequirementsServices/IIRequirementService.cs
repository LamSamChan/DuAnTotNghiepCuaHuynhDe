using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices
{
    public interface IIRequirementService
    {
        Task<List<InstallationRequirement>> GetAll();
    }
}
