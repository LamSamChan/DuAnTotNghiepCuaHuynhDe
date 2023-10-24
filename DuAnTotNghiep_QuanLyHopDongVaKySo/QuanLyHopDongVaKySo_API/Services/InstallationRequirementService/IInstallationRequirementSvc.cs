using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.InstallationRequirementService
{
    public interface IInstallationRequirementSvc
    {
        Task<List<InstallationRequirement>> GetAll();
        Task<int> CreateIRequirement(InstallationRequirement installationRequirement);
        Task<InstallationRequirement> GetById(int installationRequirementID);
        Task<int> DeleteIRequirement(int installationRequirementID);
    }
}
