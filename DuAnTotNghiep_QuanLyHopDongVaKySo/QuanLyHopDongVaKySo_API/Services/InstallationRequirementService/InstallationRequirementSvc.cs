using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_API.Services.InstallationRequirementService
{
    public class InstallationRequirementSvc : IInstallationRequirementSvc
    {
        private readonly ProjectDbContext _context;

        public InstallationRequirementSvc(ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateIRequirement(InstallationRequirement installationRequirement)
        {
            try
            {
                int isSuccess = 0;
                _context.InstallationRequirements.Add(installationRequirement);
                await _context.SaveChangesAsync();
                isSuccess = installationRequirement.InstallRequireID;
                return isSuccess;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<List<InstallationRequirement>> GetAll()
        {
            try
            {
                return await _context.InstallationRequirements.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<InstallationRequirement>();
            }
        }

        public async Task<InstallationRequirement> GetById(int installationRequirementID)
        {
            try
            {
                return _context.InstallationRequirements.FirstOrDefault(r => r.InstallRequireID == installationRequirementID);
            }
            catch (Exception ex)
            {
                return new InstallationRequirement();
            }
        }
    }
}
