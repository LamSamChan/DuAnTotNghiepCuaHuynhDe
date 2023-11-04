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


        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService) { 
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPosition()
        {
            return View();
        }
        public IActionResult AddPosition_2()
        {
            return View();
        }
        public IActionResult AddPosition_3()
        {
            return View();
        }
        public IActionResult AddPosition_4()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }
        public IActionResult AddRole_2()
        {
            return View();
        }
        public IActionResult AddRole_3()
        {
            return View();
        }
        public IActionResult AddRole_4()
        {
            return View();
        }
        public IActionResult ListPrivate()
        {
            return View();
        }
        public IActionResult ListPersonnel()
        {
            return View();
        }
        public IActionResult DetailsAccount()
        {
            return View();
        }
        public IActionResult ListAccount()
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
