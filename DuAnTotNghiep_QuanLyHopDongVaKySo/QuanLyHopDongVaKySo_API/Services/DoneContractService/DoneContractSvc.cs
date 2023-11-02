using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using Microsoft.EntityFrameworkCore;
namespace QuanLyHopDongVaKySo_API.Services.DoneContractService
{
    public class DoneContractSvc : IDoneContractSvc
    {
        private readonly ProjectDbContext _context;

        public DoneContractSvc (ProjectDbContext context)
        {
            _context = context;
        }
        public async Task<DoneContract> addAsnyc(PutPendingContract pContract)
        {
            try{

                DoneContract add = new DoneContract()
                {
                    DateDone = DateTime.Now,
                    DConTractName = pContract.PContractName,
                    DContractFile = pContract.PContractFile,
                    InstallationAddress = pContract.InstallationAddress,
                    IsInEffect = true,
                    EmployeeCreatedId = pContract.EmployeeCreatedId,
                    DirectorSignedId = (Guid)pContract.DirectorSignedId,
                    CustomerId = pContract.CustomerId,
                    TOS_ID = pContract.TOS_ID,
                };
                await _context.DoneContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add;
            }catch
            {
                return new DoneContract();
            }
        }


        public async Task<List<DoneContract>> getAllAsnyc()
        {
            return await _context.DoneContracts.ToListAsync();
        }

        public async Task<DoneContract> getByIdAsnyc(int id)
        {
            return await _context.DoneContracts.Where(D => D.DContractID == id).FirstOrDefaultAsync();
        }

        public Task<string> updateAsnyc(PutDContract dContract)
        {
            throw new NotImplementedException();
        }
    }
}
