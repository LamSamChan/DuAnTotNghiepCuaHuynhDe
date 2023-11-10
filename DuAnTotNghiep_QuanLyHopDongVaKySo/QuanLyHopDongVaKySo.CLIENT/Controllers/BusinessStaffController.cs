using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo_CLIENT.Constants;
using System.Drawing.Imaging;
using test.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IDContractsService _dContractService;
        private readonly IPContractService _pContractService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private int isAuthenticate  = 1 ;
        private string employeeId ;

        public int IsAuthenticate
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID)))
                {
                    string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
                    if (role == "Admin")
                    {
                        isAuthenticate = 1; //Admin
                    }
                    else if (role == "Giám đốc")
                    {
                        isAuthenticate = 2; //Director
                    }
                    else if (role == "Nhân viên kinh doanh")
                    {
                        isAuthenticate = 3; //BusinessStaff
                    }
                    else if (role == "Nhân viên lắp đặt")
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

        public BusinessStaffController(ICustomerService customerService, IDContractsService dContractService,IEmployeeService employeeService,
            IPContractService pContractService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor)
        {
            _customerService = customerService;
            _dContractService = dContractService;
            _pContractService = pContractService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
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
        public IActionResult CreateFormForCus()
        {
            return View();
        }
        public async Task<IActionResult> DetailsCus(string customerID)
        {
            var respone = await _customerService.GetCustomerById(customerID);
            if (respone != null)
            {
                VMDetailsCus vm = new VMDetailsCus();
                vm.Customer = respone;

                //truyền thêm
                vm.PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.CustomerId == customerID).ToList();
                vm.DoneContracts = await _dContractService.getListByCusId(customerID);
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }
        public IActionResult DetailsDContract()
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
                contractList = await _dContractService.getListByDirectorId(EmployeeId);

                return View(contractList);
            }
            else if (isAuthenticate == 3)
            {
                contractList = await _dContractService.getListByEmpId(EmployeeId);

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
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            if (isAuthenticate == 1)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.IsDirector == "Chờ ký").ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == EmployeeId && p.IsDirector == "Chờ ký").ToList();
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
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            if (isAuthenticate == 1 || isAuthenticate == 2)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.IsRefuse == "Từ chối").ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == EmployeeId && p.IsRefuse == "Từ chối").ToList();
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
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            if (isAuthenticate == 1)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.IsCustomer == "Chờ ký").ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == EmployeeId && p.IsCustomer == "Chờ ký" && p.IsDirector == "Đã ký").ToList();
                return View(pContractList);
            }
            else if (isAuthenticate == 2)
            {
                pContractList = _pContractService.getAllAsnyc().Result.Where(p => p.DirectorSignedId == EmployeeId && p.IsCustomer == "Chờ ký").ToList();
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

        public async Task<IActionResult> DetailsContractEffect(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();
            try 
            { 
            viewModel.DoneContracts = _dContractService.getAllAsnyc().Result.Where(d => d.Id== id).FirstOrDefault();
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.DoneContracts.CustomerId);
            }
            catch
            {
                return RedirectToAction("Index");
            }
                return View(viewModel);
        }

        public async Task<IActionResult>  DetailsContractPending(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();
            try 
            { 
            viewModel.PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.PContractID == id).FirstOrDefault();
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
            viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.EmployeeCreatedId);
            }
            catch
            {
                return RedirectToAction("Index");
            }
                return View(viewModel);
        }

        public async Task<IActionResult> DetailsContractRefuse(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();

            try 
            { 
                viewModel.PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.PContractID == id).FirstOrDefault();
                viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
                viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.DirectorSignedId);
            }
            catch
            {
                return RedirectToAction("Index");
            }
                return View(viewModel);
        }

        public async Task<IActionResult> DetailsContractWaitSign(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();

            try
            {
                viewModel.PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.PContractID == id).FirstOrDefault();
                viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
                viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.EmployeeCreatedId);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> EditCus(string customerID)
        {
            var customer = await _customerService.GetCustomerByIdPut(customerID);
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
            var customer = await _customerService.GetCustomerByIdPut(respone);
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

        [HttpPost]
        public ActionResult SaveSignature([FromBody] SignData sData)
        {
            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid() + ".png";

            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "SignatureImages"), fileName);


            bmpSign.Save(filePath, ImageFormat.Png);

            return Content(fileName);
        }
    }
}