using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public async Task<List<DContractViewModel>> getListIsEffect()
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts
                .Select(dc => new DContractViewModel
                {
                    id = dc.DContractID.ToString(),
                    fullName = dc.Customer.FullName,
                    enail = dc.Customer.Email,
                    dateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    typeOfService = dc.TypeOfService.ServiceName,
                    status = dc.IsInEffect ? "Hoat dong" : ""
                }).ToListAsync();
            return viewModel;
        }

        public async Task<string> updateAsnyc(PutDContract dContract)
        {
            string ret = null;
            var update = await getByIdAsnyc(dContract.DContractID);
            try
            {
                if (update != null)
                {
                    update.DContractID = dContract.DContractID;
                    update.DateDone = dContract.DateDone;
                    update.DConTractName = dContract.DContractName;
                    update.DContractFile = dContract.DContractFile;
                    update.IsInEffect = dContract.IsInEffect;
                    update.InstallationAddress = dContract.InstallationAddress;
                    update.EmployeeCreatedId = dContract.EmployeeCreatedId;
                    update.DirectorSignedId = dContract.DirectorSignedId;
                    update.CustomerId = dContract.CustomerId;
                    update.TOS_ID = dContract.TOS_ID;
                    update.DoneMinuteId = dContract.DoneMinuteId;
                }
                _context.DoneContracts.Update(update);
                await _context.SaveChangesAsync();
                return update.DContractID.ToString();
            }
            catch
            {
                return ret;
            }
        }
    }
}
