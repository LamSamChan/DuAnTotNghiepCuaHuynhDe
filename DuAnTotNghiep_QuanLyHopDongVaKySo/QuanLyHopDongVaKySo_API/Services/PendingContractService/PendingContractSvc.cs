using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Drawing;

namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public class PendingContractSvc : IPendingContractSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IPositionSvc _positionSvc;
        private readonly ICustomerSvc _customerSvc;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        public PendingContractSvc(ProjectDbContext context, IUploadFileHelper imageHelper, 
            ICustomerSvc customerSvc, ITypeOfServiceSvc typeOfServiceSvc, IPositionSvc positionSvc)
        {
            _context = context;
            _customerSvc = customerSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _positionSvc = positionSvc;
        }
        public async Task<string> addAsnyc(PostPendingContract PContract)
        {
            string serviceName = _typeOfServiceSvc.GetById(PContract.TOS_ID).Result.ServiceName;
            string ret = null;
            try{
                PendingContract add = new PendingContract()
                {
                    DateCreated = DateTime.Now,
                    PContractName = "Hợp đồng " + serviceName,
                    PContractFile = "",
                    InstallationAddress = PContract.InstallationAddress,
                    IsDirector = false,
                    IsCustomer = false,
                    IsRefuse = false,
                    Reason = "",
                    EmployeeCreatedId = PContract.EmployeeCreatedId,
                    DirectorSignedId = null,
                    CustomerId = PContract.CustomerId,
                    TOS_ID = PContract.TOS_ID,
                };
                await _context.PendingContracts.AddAsync(add);
                await _context.SaveChangesAsync();
                return add.PContractID.ToString();
            }catch
            {
                return ret;
            }
        }

        public async Task<bool> deleteAsnyc(int id)
        {
            var delete = await getByIdAsnyc(id);
            if(delete != null)
            {
                _context.PendingContracts.Remove(delete);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<PendingContract> getByIdAsnyc(int id)
        {
            return await _context.PendingContracts.Where(p => p.PContractID == id).FirstOrDefaultAsync();
        }

        public async Task<List<PContractViewModel>> getAllAsnyc()
        {
            List<PContractViewModel> viewModel = new List<PContractViewModel>();
            try
            {
                viewModel = await _context.PendingContracts
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File
                }).ToListAsync();
            }
            catch
            {
                
            }
            
            return viewModel;
        }
       
        public async Task<int> updatePContractFile(int id, string File, string base64File)
        {
            var update = await getByIdAsnyc(id);

            update.PContractFile = File;
            update.Base64File = base64File;
            _context.PendingContracts.Update(update);
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<string> updateAsnyc(PutPendingContract PContract)
        {
            string ret = null;
            var update = await getByIdAsnyc(PContract.PContractId);
            try{
                if(update != null)
                {
                    update.DirectorSignedId = PContract.DirectorSignedId;
                    update.IsDirector = PContract.IsDirector;
                    if (PContract.PContractFile != null)
                    {
                        update.PContractFile = PContract.PContractFile;
                    }
                    else
                    {
                        update.PContractFile = update.PContractFile;
                    }
                    update.IsRefuse = PContract.IsRefuse;
                    update.Reason = PContract.Reason;
                    update.IsCustomer = PContract.IsCustomer;
                    update.Base64File = PContract.Base64File;
                }
                _context.PendingContracts.Update(update);
                await _context.SaveChangesAsync();
                return update.PContractID.ToString();
            }catch
            {
                return ret;
            }
        }

        public async Task<ContractInternet> ExportContract(PendingContract PContract, Employee? employee)
        {
            string? postion = null;
            if (employee != null)
            {
                postion = _positionSvc.GetById(employee.PositionID)?.Result.PositionName;
            }

            Customer cus = await _customerSvc.GetByIdAsync(PContract.CustomerId.ToString());
            TypeOfService tos = await _typeOfServiceSvc.GetById(PContract.TOS_ID);
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            ContractInternet contract = new ContractInternet();
            if(cus != null)
            {
                //contract.CustomerId = cus.CustomerId.ToString();
                contract.CustomerId ="KH"+ cus.CustomerId.ToString().Substring(0,8);
                contract.ContractId ="HĐ"+ PContract.PContractID.ToString();
                contract.Date = PContract.DateCreated.ToString("dd/MM/yyyy");
                contract.BuisinessName = cus.BuisinessName != null ? cus.BuisinessName : cus.FullName;
                contract.FullName = cus.FullName;
                contract.Position = cus.Position;
                contract.DateOfBirth = cus.DateOfBirth.ToString("dd/MM/yyyy");
                contract.PhoneNumber = cus.PhoneNumber;
                contract.Email = cus.Email;
                contract.PowerOfAttorneyNum = cus.PowerOfAttorneyNum;
                contract.WhoPOA = cus.WhoPOA;
                contract.DatePOA = cus.DatePOA?.ToString("dd/MM/yyyy");
                contract.BuisinessNumber = cus.BuisinessNumber;
                contract.BNDate = cus.BNDate?.ToString("dd/MM/yyyy");
                contract.BNPlace = cus.BNPlace;
                contract.DatePOA = cus.DatePOA?.ToString("dd/MM/yyyy");
                contract.Identification = cus.Identification;
                contract.IssuedDate = cus.IssuedDate.ToString("dd/MM/yyyy");
                contract.IssuedPlace = cus.IssuedPlace;
                contract.Nationality = cus.Nationality;
                contract.BankAccount = cus.BankAccount;
                contract.BankName = cus.BankName;
                contract.TaxIDNumber = cus.TaxIDNumber;
                contract.Address = cus.Address;
                contract.FAX = cus.FAX;
                contract.ChargeNoticeAddress = cus.ChargeNoticeAddress;
                contract.BillingAddress = cus.BillingAddress;
                contract.Username = cus.FullName.Replace(" ", "").Trim();
                contract.TariffPackage = tos.ServiceName;
                contract.ServiceRate = String.Format(info, "{0:c}", tos.Price)+ " / " + tos.PerTime;
                contract.InstallationAddress = PContract.InstallationAddress;
                contract.RepresentativePerson1 = employee.FullName;
                contract.RepresentativePerson2 = employee.FullName;
                contract.RepresentativePosition1 = postion == null ? "" : postion;
                contract.RepresentativePosition2 = postion == null ? "" : postion;
                contract.CustomerSignName = cus.FullName;
                contract.CustomerSignPosition = cus.Position;
            }
            return contract;
        }

        public async Task<List<PContractViewModel>> getListWaitDirectorSigns()
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.IsDirector == false && p.IsCustomer == false && !p.IsRefuse)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch(Exception ex)
            {

            }
            
            return viewModels;
        }
       
        public async Task<List<PContractViewModel>> getListWaitCustomerSigns()
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.IsDirector == true && p.IsCustomer == false)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch(Exception ex)
            {

            }
            
            return viewModels;
        }

        public async Task<PContractViewModel> geByIdView(int id)
        {
            PContractViewModel viewModel = new PContractViewModel();
            try
            {
                viewModel = await _context.PendingContracts.Where(p => p.PContractID == id).
                Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).FirstOrDefaultAsync();
            }
            catch
            {

            }
            
            
            return viewModel;
        }

        public async Task<List<PContractViewModel>> getListEmpId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                
                viewModels = await _context.PendingContracts.Where(p => p.EmployeeCreatedId == Guid.Parse(id))
                    .Select(pc => new PContractViewModel
                    {
                        PContractID = pc.PContractID.ToString(),
                        PContractName = pc.PContractName,
                        DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                        CustomerName = pc.Customer.FullName,
                        CustomerEmail = pc.Customer.Email,
                        IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                        IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                        CustomerId = pc.CustomerId.ToString().ToLower(),
                        IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                        DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                        EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                        Reason = pc.Reason,
                        InstallationAddress = pc.InstallationAddress,
                        TOS_ID = pc.TypeOfService.ServiceName,
                        PContractFile = pc.PContractFile,
                        Base64File = pc.Base64File
                        
                    }).ToListAsync();
            }
            catch
            {

            }
            return viewModels;
        }

        public async Task<List<PContractViewModel>> getListCusId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.CustomerId == Guid.Parse(id))
                    .Select(pc => new PContractViewModel
                    {
                        PContractID = pc.PContractID.ToString(),
                        PContractName = pc.PContractName,
                        DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                        CustomerName = pc.Customer.FullName,
                        CustomerEmail = pc.Customer.Email,
                        IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                        IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                        CustomerId = pc.CustomerId.ToString().ToLower(),
                        IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                        DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                        EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                        Reason = pc.Reason,
                        InstallationAddress = pc.InstallationAddress,
                        TOS_ID = pc.TypeOfService.ServiceName,
                        PContractFile = pc.PContractFile,
                        Base64File = pc.Base64File

                    }).ToListAsync();
            }
            catch { }
            
            return viewModels;
        }

        public async Task<List<PContractViewModel>> getListRefuse()
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.IsRefuse == true)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch { }
            
            return viewModels;
        }

        public async Task<List<PContractViewModel>> getListRefuseByEmpId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.IsRefuse == true && p.EmployeeCreatedId == Guid.Parse(id))
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch { }
            
            return viewModels;
        }

        public async Task<List<PContractViewModel>> getListWaitCusSignsByEmpId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.EmployeeCreatedId == Guid.Parse(id) && p.IsDirector == true && p.IsCustomer == false)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch(Exception ex) { }
            
            return viewModels;
        }

        public async Task<List<PContractViewModel>> getListWaitCusSignsByDirId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.DirectorSignedId == Guid.Parse(id) && p.IsDirector == true && p.IsCustomer == false)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch(Exception ex) { }
            
            return viewModels;
        }
        public async Task<List<PContractViewModel>> getListWaitDirSignsByEmpId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.EmployeeCreatedId == Guid.Parse(id) && p.IsDirector == false && p.IsCustomer == false)
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch(Exception ex) { }
            
            return viewModels;
        }
        public async Task<List<PContractViewModel>> getListDirSignsByEmpId(string id)
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            try
            {
                viewModels = await _context.PendingContracts.Where(p => p.DirectorSignedId == Guid.Parse(id))
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    PContractName = pc.PContractName,
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    CustomerName = pc.Customer.FullName,
                    CustomerEmail = pc.Customer.Email,
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chờ ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chờ ký",
                    CustomerId = pc.CustomerId.ToString().ToLower(),
                    IsRefuse = pc.IsRefuse ? "Từ chối" : "Chờ ký",
                    DirectorSignedId = pc.DirectorSignedId.ToString().ToLower(),
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString().ToLower(),
                    Reason = pc.Reason,
                    InstallationAddress = pc.InstallationAddress,
                    TOS_ID = pc.TypeOfService.ServiceName,
                    PContractFile = pc.PContractFile,
                    Base64File = pc.Base64File

                }).ToListAsync();
            }
            catch (Exception ex) { }

            return viewModels;
        }

        public async Task<PendingContract> getByIdForWinformAsnyc(int id, string cusId)
        {
            return await _context.PendingContracts.Where(c => c.CustomerId == Guid.Parse(cusId)).FirstOrDefaultAsync(p => p.PContractID == id);
        }
    }
}
