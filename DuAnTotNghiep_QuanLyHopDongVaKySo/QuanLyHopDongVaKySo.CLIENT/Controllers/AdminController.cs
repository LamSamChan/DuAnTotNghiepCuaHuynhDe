﻿using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Org.BouncyCastle.Ocsp;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly ICustomerService _customerService;
        private readonly IPFXCertificateServices _pFXCertificateServices;
        private readonly IPContractService _pContractService;
        private readonly IDContractsService _doneContractSvc;
        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService,
            ICustomerService customerService, IPFXCertificateServices pFXCertificateServices, IPContractService pContractService, IDContractsService doneContractSvc)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
            _customerService = customerService;
            _pFXCertificateServices = pFXCertificateServices;
            _pContractService= pContractService;
            _doneContractSvc = doneContractSvc;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListTypeOfService()
        {
            return View();
        }
        public IActionResult AddTypeOfService()
        {
            return View();
        }
        public IActionResult DetailsTypeOfService()
        {
            return View();
        }
        public IActionResult EditTypeOfService()
        {
            return View();
        }
        public async Task<IActionResult> ListPFXCertificate()
        {
            VMListPFX vm = new VMListPFX();
            vm.Customers = await _customerService.GetAllCustomers();
            vm.Employees = await _employeeService.GetAllEmployees();
            vm.PFXCertificates = await _pFXCertificateServices.GetAll();
            vm.PFXCertificatesE = await _pFXCertificateServices.GetAllExpire();
            vm.PFXCertificatesATE = await _pFXCertificateServices.GetAllAboutToExpire();
            return View(vm);
        }
        public async Task<IActionResult> DetailsPFXCertificate(string serial)
        {
            var respone = await _pFXCertificateServices.GetById(serial);
            return View(respone);
        }
     
        public async Task<IActionResult> UpdateNotAfterPFX(string serial)
        {
            var respone = await _pFXCertificateServices.UpdateNotAfter(serial);
            if (respone != null)
            {
                //update thành công
                return RedirectToAction("ListPFXCertificate");

            }
            else
            {
                return RedirectToAction("ListPFXCertificate");
            }
        }
        public IActionResult DetailsPosition()
        {
            return View();
        }
        public IActionResult DetailsRole()
        {
            return View();
        }
        public async Task<IActionResult> DetailsEmpAccount(string empId)
        {
            VMDetailsEmpAccount vm = new VMDetailsEmpAccount()
            {
                Employee = await _employeeService.GetEmployeeById(empId),
                Roles = await _roleService.GetAllRolesAsync(),
                Positions = await _positionService.GetAllPositionsAsync(),
                //truyền thêm pcontract + donecontract
                PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId== empId).ToList(),
                DoneContracts = _doneContractSvc.getAllAsnyc().Result.Where(d => d.EmployeeCreatedId == empId).ToList(),

            }; 
            return View(vm);
        }
        public async Task<IActionResult> EditEmpAccount(string empId)
        {
            VMEditEmployee vm = new VMEditEmployee();
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();
            vm.Employee= await _employeeService.GetEmployeePutById(empId);

            if (vm.Employee != null)
            {
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> EditEmpAction(PutEmployee employee)
        {

            if (employee.ImageFile != null)
            {
                if (employee.ImageFile.ContentType.StartsWith("image/"))
                {
                    if (employee.ImageFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            employee.ImageFile.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            employee.Base64String = Convert.ToBase64String(bytes);
                            employee.ImageFile = null;
                        }
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    RedirectToAction("AddEmpAccount");
                }
            }
            string respone = await _employeeService.UpdateEmployee(employee);
            var emp = await _employeeService.GetEmployeePutById(respone);

            if (emp != null)
            {
                VMEditEmployee vm = new VMEditEmployee();
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Employee = emp;
                return View("EditEmpAccount", vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddEmpAccount()
        {
            VMAddEmployee vm = new VMAddEmployee();
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();

            return View(vm);
        }

        public async Task<IActionResult> AddEmpAction(PostEmployee employee)
        {
            if (employee.ImageFile != null)
            {
                if (employee.ImageFile.ContentType.StartsWith("image/"))
                {
                    if (employee.ImageFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            employee.ImageFile.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            employee.Base64String = Convert.ToBase64String(bytes);
                            employee.ImageFile = null;
                        }
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    RedirectToAction("AddEmpAccount");
                }
            }
            var reponse = await _employeeService.AddNewEmployee(employee);
            
            if (reponse != 0)
            {
                return RedirectToAction("ListUsersAccount");
            }
            else
            {
                return RedirectToAction("AddEmpAccount");
            }
        }

        public async Task<IActionResult> ListUsersAccount()
        {
            VMAdminListUsersAccount vm = new VMAdminListUsersAccount();
            try
            {
                vm.Customers = await _customerService.GetAllCustomers();
                vm.Employees = await _employeeService.GetAllEmployees();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Roles = await _roleService.GetAllRolesAsync();
                return View(vm);
            }
            catch (Exception ex)
            {

                return View(new VMAdminListUsersAccount());
            }
        }



        public async Task<IActionResult> ListPosition()
        {
            VMListPosition vm = new VMListPosition();
            try
            {
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Employees = await _employeeService.GetAllEmployees();
                return View(vm);
            }
            catch (Exception ex)
            {

                return View(new VMListPosition());
            }
        }
        public async Task<IActionResult> ListRole()
        {
            VMListRole vm = new VMListRole();
            try
            {
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Employees = await _employeeService.GetAllEmployees();
                return View(vm);
            }
            catch (Exception ex)
            {

                return View(new VMListRole());
            }
        }

        public async Task<IActionResult> AddPosition(Position position)
        {
            int reponse = await _positionService.AddPositionAsync(position);
            if (reponse != 0)
            {
                return RedirectToAction("ListPosition");
            }
            else
            {
                return RedirectToAction("ListPosition");
            }
        }

        public async Task<IActionResult> EditPosition(int positionId)
        {
            var position = await _positionService.GetPositionByIdAsync(positionId);
            if (position != null)
            {
                return View(position);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> UpdatePosition(Position position)
        {
            var update = await _positionService.UpdatePositionAsync(position);
            if (update != 0)
            {
                return RedirectToAction("ListPosition");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddRole(Role role)
        {

            int reponse = await _roleService.AddRoleAsync(role);
            if (reponse != 0)
            {
                return RedirectToAction("ListRole");
            }
            else
            {
                return RedirectToAction("ListPosition");
            }
        }

        public async Task<IActionResult> EditRole(int roleId)
        {
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role != null)
            {
                return View(role);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public async Task<IActionResult> UpdateRole(Role role)
        {
            var update = await _roleService.UpdateRoleAsync(role);
            if (update != 0)
            {
                return RedirectToAction("ListRole");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}
