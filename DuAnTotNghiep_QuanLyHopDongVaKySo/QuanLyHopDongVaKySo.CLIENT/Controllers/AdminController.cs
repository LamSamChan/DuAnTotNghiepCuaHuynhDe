using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;

        public AdminController(IPositionService positionService, IEmployeeService employeeService) { 
            _positionService = positionService;
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPosition()
        {
            return View();
        }
        public IActionResult AddRole()
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

                return View(ex);
            }
        }
        public IActionResult ListRole()
        {
            return View();
        }
    }
}
