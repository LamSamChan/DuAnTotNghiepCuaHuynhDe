﻿using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
namespace QuanLyHopDongVaKySo_API.Services.PendingContractService
{
    public class PendingContractSvc : IPendingContractSvc
    {
        private readonly ProjectDbContext _context;
        private readonly IUploadFileHelper _imageHelper;
        private readonly ICustomerSvc _customerSvc;
        public PendingContractSvc(ProjectDbContext context, IUploadFileHelper imageHelper, ICustomerSvc customerSvc)
        {
            _context = context;
            _imageHelper = imageHelper;
            _customerSvc = customerSvc;
        }
        public async Task<string> addAsnyc(PostPendingContract PContract)
        {
            string ret = null;
            try{
                PendingContract add = new PendingContract()
                {
                    DateCreated = DateTime.Now,
                    PContractName = "Hợp đồng sử dụng" + (PContract.TOS_ID == 1? "internet": "Dien thoai").ToString(),
                    PContractFile = "",
                    IsDirector = false,
                    IsCustomer = false,
                    IsRefuse = false,
                    Reason = PContract.Reason,
                    EmployeeCreatedId = PContract.EmployeeCreatedId,
                    DirectorSignedId = null,
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
                    update.DateCreated = PContract.DateCreated;
                    update.PContractName = PContract.PContractName;
                    update.PContractFile = PContract.PContractFile;
                    update.IsDirector = PContract.IsDirector;
                    update.IsCustomer = PContract.IsCustomer;
                    update.IsRefuse = PContract.IsRefuse;
                    update.Reason = PContract.Reason;
                    update.EmployeeCreatedId = PContract.EmployeeCreatedId;
                    update.DirectorSignedId = PContract.DirectorSignedId;
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

        public async Task<ContractInternet> ExportContract(PendingContract PContract)
        {
            Customer cus = await _customerSvc.GetByIdAsync(PContract.CustomerId.ToString());

            ContractInternet contract = new ContractInternet();
            if(cus != null)
            {
                //contract.CustomerId = cus.CustomerId.ToString();
                contract.CustomerId = cus.CustomerId.ToString();
                contract.ContractId = PContract.PContractID.ToString();
                contract.Date = PContract.DateCreated.ToString();
                contract.BuisinessName = cus.BuisinessName;
                contract.FullName = cus.FullName;
                contract.Position = cus.Position;
                contract.DateOfBirth = cus.DateOfBirth;
                contract.PhoneNumber = cus.PhoneNumber;
                contract.Email = cus.Email;
                contract.Identification = cus.Identification;
                contract.IssuedDate = cus.IssuedDate;
                contract.IssuedPlace = cus.IssuedPlace;
                contract.Nationality = cus.Nationality;
                contract.BankAccount = cus.BankAccount;
                contract.BankName = cus.BankName;
                contract.TaxIDNumber = cus.TaxIDNumber;
                contract.Address = cus.Address;
                contract.ChargeNoticeAddress = "25 phung van cung";
                contract.InvoiceIssuingAddress = "25 phung van cung";
                contract.Username = "Danhkunz";
                contract.TariffPackage = "Goi intetnet gia dinh";
                contract.ServiceRate = "25000000";
                contract.InstallationAddress = "25 phung van cung";
            }
            return contract;
        }
    }
}
