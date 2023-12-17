using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Data;
using Microsoft.IdentityModel.Tokens;

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
                    DateUnEffect = default,
                    DConTractName = pContract.PContractName,
                    DContractFile = pContract.PContractFile,
                    InstallationAddress = pContract.InstallationAddress,
                    IsInEffect = false,
                    EmployeeCreatedId = pContract.EmployeeCreatedId,
                    DirectorSignedId = (Guid)pContract.DirectorSignedId,
                    CustomerId = pContract.CustomerId,
                    TOS_ID = pContract.TOS_ID,
                    Base64File = pContract.Base64File,
                };
                await _context.DoneContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add;
            }catch
            {
                return new DoneContract();
            }
        }

        public async Task<List<DoneContract>> GetAll()
        {
            try
            {
                return await _context.DoneContracts.ToListAsync();
            }
            catch (Exception ex)
            {

                return new List<DoneContract>();
            }
        }

        public async Task<List<DContractViewModel>> getAllAsnyc()
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName
                }).ToListAsync();
            return viewModel;
        }

        public async Task<DoneContract> getByIdAsnyc(string id)
        {
            return await _context.DoneContracts.Where(D => D.DContractID ==int.Parse(id)).FirstOrDefaultAsync();
        }

        public async Task<DContractViewModel> getByIView(int id)
        {
            DContractViewModel viewModel = new DContractViewModel();

            viewModel = await _context.DoneContracts.Where(d => d.DContractID == id)
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName


                }).FirstOrDefaultAsync();
            return viewModel;
        }

        public async Task<List<DContractViewModel>> getListByCusId(string id)
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts.Where(d => d.CustomerId == Guid.Parse(id))
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName

                }).ToListAsync();
            return viewModel;
        }


        public async Task<List<DContractViewModel>> getListByDirectorId(string id)
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts.Where(d => d.DirectorSignedId == Guid.Parse(id))
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId= dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId= dc.CustomerId.ToString().ToLower(),
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName

                }).ToListAsync();
            return viewModel;
        }

        public async Task<List<DContractViewModel>> getListByEmpId(string id)
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts.Where(d => d.EmployeeCreatedId == Guid.Parse(id))
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName

                }).ToListAsync();
            return viewModel;
        }

        

        public async Task<List<DContractViewModel>> getListIsEffect()
        {
            List<DContractViewModel> viewModel = new List<DContractViewModel>();

            viewModel = await _context.DoneContracts.Where(d => d.IsInEffect)
                .Select(dc => new DContractViewModel
                {
                    Id = dc.DContractID.ToString(),
                    CustomerName = dc.Customer.FullName,
                    CustomerEmail = dc.Customer.Email,
                    DateDone = dc.DateDone.ToString("dd/MM/yyyy"),
                    DateUnEffect = !String.IsNullOrEmpty(dc.DateUnEffect.ToString()) ? dc.DateUnEffect.ToString("dd/MM/yyyy") : null,
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã dừng hiệu lực",
                    Base64File = dc.Base64File,
                    DMinuteID = dc.DoneMinuteId.ToString(),
                    DContractName = dc.DConTractName
                }).ToListAsync();
            return viewModel;
        }


        public async Task<string> updateAsnycDMinute(PutDContract dContract)
        {
            string ret = null;
            var update = await getByIdAsnyc(dContract.DContractID);
            try
            {
                if (update != null)
                {
                    update.IsInEffect = dContract.IsInEffect;
                    if (dContract.DoneMinuteId != null)
                    {
                        update.DoneMinuteId = dContract.DoneMinuteId;
                    }
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

        public async Task<DoneContract> updateAsnyc(PutDContract dContract)
        {
            string ret = null;
            var update = await getByIdAsnyc(dContract.DContractID);
            try
            {
                if (update != null)
                {
                        update.DateDone = DateTime.Now;
                        update.DConTractName = dContract.DContractName;
                        update.DContractFile = dContract.DContractFile;
                        update.IsInEffect = dContract.IsInEffect;
                        update.InstallationAddress = dContract.InstallationAddress;
                        update.CustomerId = dContract.CustomerId;
                        update.EmployeeCreatedId = dContract.EmployeeCreatedId;
                        update.DirectorSignedId = dContract.DirectorSignedId;
                        update.TOS_ID = dContract.TOS_ID;
                        update.Base64File = dContract.Base64File;
                }
                _context.DoneContracts.Update(update);
                await _context.SaveChangesAsync();
                return update;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DoneContract> updateIsEffect(PutDContract dContract)
        {
            string ret = null;
            var update = await getByIdAsnyc(dContract.DContractID);
            try
            {
                if (update != null)
                {
                    update.DateUnEffect = DateTime.Now;
                    update.IsInEffect = false;                    
                }
                _context.DoneContracts.Update(update);
                await _context.SaveChangesAsync();
                return update;
            }
            catch
            {
                return null;
            }
        }

        public async Task<DoneContract> AddDContractFromSignByUSBToken(DoneContract dContract)
        {
            try
            {
                _context.DoneContracts.Add(dContract);
                await _context.SaveChangesAsync();
                return dContract;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DoneContract> UpdateContractFromSignByUSBToken(DoneContract dContract)
        {
            try
            {
                _context.DoneContracts.Update(dContract);
                await _context.SaveChangesAsync();
                return dContract;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<DoneContract>> getNotInstallYet()
        {
            try
            {
                return _context.DoneContracts.ToListAsync().Result.Where(d => d.DoneMinuteId == null).ToList();
            }
            catch (Exception ex)
            {

                return new List<DoneContract>();
            }
        }
    }
}
