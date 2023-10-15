using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;

namespace QuanLyHopDongVaKySo_API.Services.TemplateContractService
{
    public class TemplateContractSvc : ITemplateContractSvc
    {
        private readonly ProjectDbContext _context;
        private UploadImageHelper _helpers;
        public TemplateContractSvc(ProjectDbContext context,UploadImageHelper helpers)
        {
            _context = context;
            _helpers = helpers;
        }
        public async Task<int> addTContract(PostTContract tContract)
        {
            try{
                if(tContract.File != null)
                {
                    _helpers.UploadImage(tContract.File,"data","TContract");
                }
                TemplateContract add = new TemplateContract()
                {
                    DateAdded = DateTime.Now,
                    TContractName = tContract.TContractName,
                    TContractFile = "data\\TContract\\"+tContract.File.FileName,
                    jsonCustomerZone = tContract.jsonCustomerZone,
                    jsonDirectorZone = tContract.jsonDirectorZone
                };
                await _context.TemplateContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.TContactID;
            }catch
            {
                return 0;
            }
        }

        public async Task<bool> deleteTContract(int id)
        {
            var delete = await getTContractAsnyc(id);
            if(delete != null)
            {
                _context.TemplateContracts.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<TemplateContract> getTContractAsnyc(int id)
        {
            return await _context.TemplateContracts.Where(t => t.TContactID == id).FirstOrDefaultAsync();
        }

        public async Task<List<TemplateContract>> getTContractsAsnyc()
        {
            return await _context.TemplateContracts.ToListAsync();
        }

        public async Task<int> updateTContract(PutTContract tContract)
        {
            try{
                var update = await getTContractAsnyc(tContract.TContractID);
                if(tContract.File != null)
                {
                    _helpers.UploadImage(tContract.File,"data","TContract");
                }
                if(update != null)
                {
                    update.TContractName = tContract.TContractName;
                    update.TContractFile = "data\\TContract\\"+tContract.File.FileName;
                    update.DateAdded = DateTime.Now;
                    update.jsonCustomerZone = tContract.jsonCustomerZone;
                    update.jsonDirectorZone = tContract.jsonDirectorZone;
                    _context.TemplateContracts.Update(update);
                    await _context.SaveChangesAsync();
                    return update.TContactID;
                }
                return 0;
            }catch{
                return 0;
            }
        }
    }
}
