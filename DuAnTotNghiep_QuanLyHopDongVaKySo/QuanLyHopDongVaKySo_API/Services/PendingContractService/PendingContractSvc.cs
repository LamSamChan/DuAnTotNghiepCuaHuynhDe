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
                    PContractName = "Hợp đồng sử dụng " + serviceName,
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

        public async Task<List<PendingContract>> getAllAsnyc()
        {
            return await _context.PendingContracts.ToListAsync();
        }

        public async Task<int> updatePContractFile(int id, string File)
        {
            var update = await getByIdAsnyc(id);

            update.PContractFile = File;
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
                    update.IsRefuse = PContract.IsRefuse;
                    update.Reason = PContract.Reason;
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
            }
            return contract;
        }

        public async Task<List<PContractViewModel>> getListWaitDirectorSigns()
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            viewModels = await _context.PendingContracts
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    PContractName = pc.PContractName,
                    PContractFile = pc.PContractFile,
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString(),
                    IsDirector = pc.IsDirector? "Đã ký" : "Chưa ký",
                    IsCustomer = pc.IsCustomer? "Đã ký" : "Chưa ký",
                    IsRefuse = pc.IsRefuse ? "Từ chối ký" : "Đã duyệt",
                    InstallationAddress = pc.InstallationAddress,
                    Reason = pc.Reason,
                    TOS_ID = pc.TypeOfService.ServiceName
                }).Where(p => p.IsDirector == "Chưa ký" && p.IsCustomer == "Chưa ký").ToListAsync();
            return viewModels;
        }
       
        public async Task<List<PContractViewModel>> getListWaitCustomerSigns()
        {
            List<PContractViewModel> viewModels = new List<PContractViewModel>();
            viewModels = await _context.PendingContracts
                .Select(pc => new PContractViewModel
                {
                    PContractID = pc.PContractID.ToString(),
                    DateCreated = pc.DateCreated.ToString("dd/MM/yyyy"),
                    PContractName = pc.PContractName,
                    PContractFile = pc.PContractFile,
                    EmployeeCreatedId = pc.EmployeeCreatedId.ToString(),
                    DirectorSignedId = pc.DirectorSignedId.ToString(),
                    IsDirector = pc.IsDirector ? "Đã ký" : "Chưa ký",
                    IsCustomer = pc.IsCustomer ? "Đã ký" : "Chưa ký",
                    InstallationAddress = pc.InstallationAddress,
                    IsRefuse = pc.IsRefuse? "Từ chối ký" : "Đã duyệt",
                    Reason = pc.Reason,
                    TOS_ID = pc.TypeOfService.ServiceName
                }).Where(p => p.IsDirector == "Đã ký" && p.IsCustomer == "Chưa ký").ToListAsync();
            return viewModels;
        }
      
    }
}
