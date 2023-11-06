using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
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

        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService,
            ICustomerService customerService)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
            _customerService = customerService;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailsUsers()
        {
            return View();
        }
        public IActionResult DetailsPosition()
        {
            return View();
        }
        public IActionResult DetailsRole()
        {
            return View();
        }
        public IActionResult DetailsEmpAccount()
        {
            return View();
        }
        public IActionResult EditEmpAccount()
        {
            return View();
        }
        public IActionResult EditUsers()
        {
            return View();
        }

        public IActionResult AddEmpAccount()
        {
            return View();
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
