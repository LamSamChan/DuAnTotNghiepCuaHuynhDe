using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public class PendingContractSvc : IPendingContractSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IUploadFileHelper _imageHelper;
        public PendingContractSvc(ProjectDbContext context, IUploadFileHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }
        public async Task<string> addPContractAsnyc(PostPendingContract PContract)
        {
            string ret = null;
            try{
                PendingContract add = new PendingContract()
                {
                    DateCreated = DateTime.Now,
                    PContractName = PContract.PContractName,
                    PContractFile = "",
                    IsDirector = false,
                    IsCustomer = false,
                    IsRefuse = false,
                    Reason = PContract.Reason,
                    EmployeeId = PContract.EmployeeId,
                    CustomerId = PContract.CustomerId,
                    TOS_ID = PContract.TOS_ID,
                    TContractId = PContract.TContractId
                };
                await _context.PendingContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.PContractID.ToString();
            }catch
            {
                return ret;
            }
        }

        public async Task<bool> deletePContractAsnyc(int id)
        {
            var delete = await getPContractAsnyc(id);
            if(delete != null)
            {
                _context.PendingContracts.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PendingContract> getPContractAsnyc(int id)
        {
            return await _context.PendingContracts.Where(p => p.PContractID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PendingContract>> getPContractsAsnyc()
        {
            return await _context.PendingContracts.ToListAsync();
        }

        public async Task<int> updatePContractFile(int id, string File)
        {
            var update = await getPContractAsnyc(id);

            update.PContractFile = File;
            _context.PendingContracts.Update(update);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<string> updatePContractAsnyc(PutPendingContract PContract)
        {
            string ret = null;
            var update = await getPContractAsnyc(PContract.PContractId);
            try{
                if(PContract.File != null)
                {
                    _imageHelper.UploadFile(PContract.File,"AppData","PContracts");
                }
                if(update != null)
                {
                    update.DateCreated = PContract.DateCreated;
                    update.PContractName = PContract.PContractName;
                    update.PContractFile = @"AppData\PContracts\"+PContract.File.FileName;
                    update.IsDirector = PContract.IsDirector;
                    update.IsCustomer = PContract.IsCustomer;
                    update.IsRefuse = PContract.IsRefuse;
                    update.Reason = PContract.Reason;
                    update.EmployeeId = PContract.EmployeeId;
                    update.CustomerId = PContract.CustomerId;
                    update.TOS_ID = PContract.TOS_ID;
                    update.TContractId = PContract.TContractId;
                }
                _context.PendingContracts.Update(update);
                await _context.SaveChangesAsync();
                return update.PContractID.ToString();
            }catch
            {
                return ret;
            }
        }
    }
}
