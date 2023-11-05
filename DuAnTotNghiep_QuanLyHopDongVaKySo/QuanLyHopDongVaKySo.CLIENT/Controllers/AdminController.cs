using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;


        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
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
        public IActionResult DetailsAccount()
        {
            return View();
        }
        public IActionResult EditAccount()
        {
            return View();
        }
        public IActionResult EditUsers()
        {
            return View();
        }
        public IActionResult EditRole()
        {
            return View();
        }
        public IActionResult EditPosition()
        {
            return View();
        }

        public IActionResult ListUsersAccount()
        {
            return View();
        }



        public async Task<IActionResult> ListPosition()
        {
            VMListPostion vm = new VMListPostion();
            try
            {
                vm.Postions = await _positionService.GetAllPositionsAsync();
                vm.Employees = await _employeeService.GetAllEmployees();
                return View(vm);
            }
            catch (Exception ex)
            {

                return View(new VMListPostion());
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
    }
}
