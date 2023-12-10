using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices
{
    public interface IIRequirementService
    {
        Task<List<InstallationRequirement>> GetAll();
    }
}
