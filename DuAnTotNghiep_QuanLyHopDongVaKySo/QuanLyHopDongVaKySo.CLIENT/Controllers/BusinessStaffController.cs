using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo_CLIENT.Constants;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDContractsService _dContractService;
        private readonly IPContractService _pContractService;
        private int isAuthenticate = 1 ;
        private string employeeId;

        public int IsAuthenticate
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID)))
                {
                    if (HttpContext.Session.GetString(SessionKey.Employee.Role) == "Admin")
                    {
                        isAuthenticate = 1; //Admin
                    }
                    else if (HttpContext.Session.GetString(SessionKey.Employee.Role) == "Giám đốc")
                    {
                        isAuthenticate = 2; //Director
                    }
                    else if (HttpContext.Session.GetString(SessionKey.Employee.Role) == "Nhân viên kinh doanh")
                    {
                        isAuthenticate = 3; //BusinessStaff
                    }
                    else if (HttpContext.Session.GetString(SessionKey.Employee.Role) == "Nhân viên lắp đặt")
                    {
                        isAuthenticate = 4; //InstallStaff
                    }
                }
                else
                {
                    isAuthenticate = 0; // chưa login
                }
                return isAuthenticate;
            }
            set { this.isAuthenticate = value; }
        }

        public string EmployeeId
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID)))
                {
                    employeeId = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID);
                }
                return employeeId;
            }
            set { this.employeeId = value; }
        }

        public BusinessStaffController(ICustomerService customerService, IDContractsService dContractService,
            IPContractService pContractService)
        {
            _customerService = customerService;
            _dContractService = dContractService;
            _pContractService = pContractService;
        }

        public async Task<IActionResult> Index()
        {
            List<Models.Customer> customersList = new List<Models.Customer>();
            try
            {
                customersList = await _customerService.GetAllCustomers();
                return View(customersList);
            }
            catch (Exception ex)
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
            if (isAuthenticate == 1)
            {
                contractList = await _dContractService.getListIsEffect();
                return View(contractList);
            }
            else if (isAuthenticate == 2)
            {
                contractList = await _dContractService.getListByDirectorId(employeeId);

                return View(contractList);
            }
            else if (isAuthenticate == 3)
            {
                contractList = await _dContractService.getListByEmpId(employeeId);

                return View(contractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(contractList);
            }
        }

        // hiển thị list chờ duyệt theo role và id ng tạo
        public async Task<IActionResult> ContractListPending()
        {
            List<PendingContract> pContractList = new List<PendingContract>();
            if (isAuthenticate == 1)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => !p.IsDirector).ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(employeeId) && !p.IsDirector).ToList();
                return View(pContractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(pContractList);
            }
        }

        public IActionResult PersonalPage()
        {
            return View();
        }

        //danh sách từ chối duyệt
        public async Task<IActionResult> ContractListRefuse()
        {
            List<PendingContract> pContractList = new List<PendingContract>();
            if (isAuthenticate == 1 || isAuthenticate == 2)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.IsRefuse).ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(employeeId) && p.IsRefuse).ToList();
                return View(pContractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(pContractList);
            }
        }

        //hiển thị danh sách chờ kh ký theo role và theo nhân viên tạo hoặc ng ký
        public async Task<IActionResult> ContractListWaitSign()
        {
            List<PendingContract> pContractList = new List<PendingContract>();
            if (isAuthenticate == 1)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => !p.IsCustomer).ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == Guid.Parse(employeeId) && !p.IsCustomer && p.IsDirector).ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 2)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.DirectorSignedId == Guid.Parse(employeeId) && !p.IsCustomer).ToList();
                return View(pContractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(pContractList);
            }
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