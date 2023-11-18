﻿using QuanLyHopDongVaKySo_API.Models;
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
                    Base64File = pContract.Base64File
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId= dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId= dc.CustomerId.ToString().ToLower(),
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã kết thúc",
                    EmployeeCreatedId = dc.EmployeeCreatedId.ToString().ToLower(),
                    DirectorSignedId = dc.DirectorSignedId.ToString().ToLower(),
                    CustomerId = dc.CustomerId.ToString().ToLower(),
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
                    TypeOfService = dc.TypeOfService.ServiceName,
                    Status = dc.IsInEffect ? "Đang hiệu lực" : "Đã dừng hiệu lực"
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
    }
}
