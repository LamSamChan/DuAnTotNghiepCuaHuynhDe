using Azure;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using CLIENT = QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo_API.ViewModels;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo_CLIENT.Constants;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDContractsService _dContractService;
        private readonly IPContractService _pContractService;

        public BusinessStaffController(ICustomerService customerService, IDContractsService dContractService,
            IPContractService pContractService) { 
            _customerService = customerService;
            _dContractService = dContractService;
            _pContractService = pContractService;
        }
        public async Task<IActionResult> Index()
        {
            List<CLIENT.Models.Customer> customersList = new List<CLIENT.Models.Customer>();
            try
            {
                customersList = await _customerService.GetAllCustomers();
                return View(customersList);
            }
            catch (Exception  ex)
            {

                return View(customersList);
            }
        }
        public IActionResult AddCus()
        {
            return View();
        }


        public async Task<IActionResult> AddCusAction(PostCustomer customer)
        {
            customer.IsLocked = false;
            if (customer.BuisinessName != null)
            {
                customer.typeofCustomer = "Doanh nghiệp";
            }
            else
            {
                customer.typeofCustomer = "Cá nhân";
            }
            int reponse = await _customerService.AddNewCustomer(customer);
            if (reponse != 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public IActionResult ContractFormPage()
        {
            return View();
        }
        //done contract
        public async Task<IActionResult> ContractListEffect()
        {
            List<DContractViewModel> contractList = new List<DContractViewModel>();
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            string empID = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID);
            if (role == "Admin")
            {
                try
                {
                    contractList = await _dContractService.getListIsEffect();
                }
                catch (Exception ex)
                {

                    return View(contractList);
                }
            }else if (role == "Giám đốc")
            { 
                try
                {
                    contractList = await _dContractService.getListByDirectorId(empID);
                }
                catch (Exception ex)
                {

                    return View(contractList);
                }
            }
            else if (role == "Nhân viên kinh doanh")
            {
                try
                {
                    contractList = await _dContractService.getListByEmpId(empID);
                }
                catch (Exception ex)
                {

                    return View(contractList);
                }
            }

            return View(contractList);

        }


        // hiển thị list chờ duyệt theo role và id ng tạo
        public async Task<IActionResult> ContractListPending()
        {
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            string empID = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID);

            List<PendingContract> pContractList = new List<PendingContract>();
            if (role == "Admin")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => !p.IsDirector).ToList();
            }
            else if (role == "Nhân viên kinh doanh")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(empID) && !p.IsDirector).ToList();
            }
            return View(pContractList);

        }
        public IActionResult PersonalPage()
        {
            return View();
        }

        //danh sách từ chối duyệt
        public async Task<IActionResult> ContractListRefuse()
        {
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            string empID = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID);
            List<PendingContract> pContractList = new List<PendingContract>();
            if (role == "Admin" || role =="Giám đốc")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.IsRefuse).ToList();
            }
            else if (role == "Nhân viên kinh doanh")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(empID) && p.IsRefuse).ToList();
            }
            return View(pContractList);
        }

        //hiển thị danh sách chờ kh ký theo role và theo nhân viên tạo hoặc ng ký
        public async Task<IActionResult> ContractListWaitSign()
        {
            string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
            string empID = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID);
            List<PendingContract> pContractList = new List<PendingContract>();
            if (role == "Admin")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => !p.IsCustomer).ToList();
            }
            else if (role == "Nhân viên kinh doanh")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(empID) && !p.IsCustomer && p.IsDirector).ToList();
            }
            else if (role == "Giám đốc")
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.DirectorSignedId == Guid.Parse(empID) && !p.IsCustomer).ToList();
            }
            return View(pContractList);

        }
        public IActionResult CreateFormContract()
        {
            return View();
        }
        public IActionResult DetailsContractEffect()
        {
            return View();
        }
        public IActionResult DetailsContractPending()
        {
            return View();
        }
        public IActionResult DetailsContractRefuse()
        {
            return View();
        }
        public IActionResult DetailsContractWaitSign()
        {
            return View();
        }
        public async Task<IActionResult> EditCus(string customerID)
        {
            var customer = await _customerService.GetCustomerById(customerID);
            if (customer != null)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UpdateInfoCustomer(PutCustomer putCustomer)
        {

            if (putCustomer.BuisinessName != null)
            {
                putCustomer.typeofCustomer = "Doanh nghiệp";
            }
            else
            {
                putCustomer.typeofCustomer = "Cá nhân";
            }

            string respone = await _customerService.UpdateCustomer(putCustomer);
            var customer = await _customerService.GetCustomerById(respone);
            if (customer != null)
            {
                return View("EditCus", customer);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult HistoryOperation()
        {
            return View();
        }
        public IActionResult SendMail()
        {
            return View();
        }
    }
}
