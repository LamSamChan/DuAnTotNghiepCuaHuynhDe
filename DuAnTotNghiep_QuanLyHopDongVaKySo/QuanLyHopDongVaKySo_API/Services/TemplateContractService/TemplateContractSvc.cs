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
        private readonly IUploadFileHelper _uploadFileHelper;
        public TemplateContractSvc(ProjectDbContext context, IUploadFileHelper uploadFileHelper)
        {
            _context = context;
            _uploadFileHelper = uploadFileHelper;
        }
        public async Task<int> addAsnyc(PostTContract tContract)
        {
            try{
                TemplateContract add = new TemplateContract()
                {
                    DateAdded = DateTime.Now,
                    TContractName = tContract.TContractName,
                    TContractFile = @"AppData/TContracts/"+tContract.TContractName + ".pdf",
                    jsonCustomerZone = null,
                    jsonDirectorZone = null,
                    Base64File = tContract.Base64StringFile
                    
                };
                await _context.TemplateContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.TContactID;
            }catch
            {
                return 0;
            }
        }

        public async Task<bool> deleteAsnyc(int id)
        {
            var delete = await getByIdAsnyc(id);
            if(delete != null)
            {
                _context.TemplateContracts.Remove(delete);
                await _context.SaveChangesAsync();
                _uploadFileHelper.RemoveFile(delete.TContractFile);
                return true;
            }
            return false;
        }

        public async Task<TemplateContract> getByIdAsnyc(int id)
        {
            return await _context.TemplateContracts.Where(t => t.TContactID == id).FirstOrDefaultAsync();
        }

        public async Task<List<TemplateContract>> getAllAsnyc()
        {
            return await _context.TemplateContracts.ToListAsync();
        }

        public async Task<int> updateAsnyc(PutTContract tContract)
        {
            try{
                var update = await getByIdAsnyc(tContract.TContractID);
                if(update != null)
                {
                    update.TContractName = tContract.TContractName;
                    update.TContractFile = tContract.TContractFile;
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
