using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
namespace QuanLyHopDongVaKySo_API.Services.TemplateContractService
{
    public class TemplateContractSvc : ITemplateContractSvc
    {
        private readonly ProjectDbContext _projectDbContext;
        private  UploadImageHelper _helpers;
        public TemplateContractSvc(ProjectDbContext projectDbContext,UploadImageHelper helpers)
        {
            _projectDbContext = projectDbContext;
            _helpers = helpers;
        }
        public Task<int> addTempContract(TemplateContract templateContract)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> deleteTempContract(int id)
        {
            var delete = await getById(id);
            if(delete != null)
            {
                _projectDbContext.TemplateContracts.Remove(delete);
                await _projectDbContext.SaveChangesAsync();
                return true;
            }
            else{
                return false;
            }
        }

        public async Task<TemplateContract> getById(int id)
        {
            return await _projectDbContext.TemplateContracts.Where(x =>x.TContactID == id).FirstOrDefaultAsync();
        }

        public async Task<List<TemplateContract>> getlist()
        {
            return await _projectDbContext.TemplateContracts.ToListAsync();
        }

        public Task<int> updateTempContract(TemplateContract templateContract)
        {
            throw new NotImplementedException();
        }
    }
}
