using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using Spire.Pdf.Graphics;
using System.Drawing.Imaging;
using test.Models;

using API = QuanLyHopDongVaKySo_API.Models;

using Employee = QuanLyHopDongVaKySo_API.Models.Employee;

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
        private readonly ITContractService _tContractService;
        private readonly ITOSService _tosService;
        private readonly ITMinuteService _tMinuteService;
        private readonly IInstallationDevicesService _installationDevicesService;
        private readonly IPFXCertificateServices _pfxCertificateServices;
        private readonly IRoleService _roleService;
        private readonly IPositionService _positionService;
        private readonly IUploadHelper _uploadHelper;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly IPasswordService _passwordService;
        private readonly IHistoryEmpSvc _historyEmpSvc;


        private int isAuthenticate = 0;
        private string employeeId;

        public int IsAuthenticate
        {
            get
            {
                if (!String.IsNullOrEmpty(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID)))
                {
                    string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
                    TempData["Role"] = role;
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

        public BusinessStaffController(ICustomerService customerService, IDContractsService dContractService, IEmployeeService employeeService,
            IPContractService pContractService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, ITContractService tContractService,
            ITOSService tosService, ITMinuteService tMinuteService, IInstallationDevicesService installationDevicesService, IPFXCertificateServices pfxCertificateServices,
            IRoleService roleService, IPositionService positionSerivce, IUploadHelper uploadHelper, IPdfToImageHelper pdfToImageHelper, IPasswordService passwordService,
            IHistoryEmpSvc historyEmpSvc)
        {
            _customerService = customerService;
            _dContractService = dContractService;
            _pContractService = pContractService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
            _tContractService = tContractService;
            _tosService = tosService;
            _roleService = roleService;
            _positionService = positionSerivce;
            _tMinuteService = tMinuteService;
            _installationDevicesService = installationDevicesService;
            _pfxCertificateServices = pfxCertificateServices;
            _uploadHelper = uploadHelper;
            _pdfToImageHelper = pdfToImageHelper;
            _passwordService = passwordService;
            _historyEmpSvc = historyEmpSvc;
        }

        public IActionResult ChangePass()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOTP()
        {
            var respone = await _passwordService.GetOTPChangeAsync(EmployeeId);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassAction([FromBody] ChangePassword change)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            change.EmployeeID = EmployeeId;
            var respone = await _passwordService.ChangePasswordAsync(change);
            if (respone != null)
            {
                var emp = await _employeeService.GetEmployeePutById(EmployeeId);
                emp.IsFirstLogin = false;
                var respone1 = await _employeeService.UpdateEmployee(emp);
                if (respone1 != null)
                {
                    var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                    Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thay đổi mật khẩu cá nhân.",
                        EmployeeID = employeeDoing.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);


                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = $"{employeeDoing.FullName} thay đổi mật khẩu thành công !!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                // mk cũ không đúng
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Mật khẩu cũ không đúng!!";
                return BadRequest();
            }
        }

        public async Task<IActionResult> CreatePContract(VMCreateFormForCus vm)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            PostPendingContract pContract = new PostPendingContract();
            pContract = vm.PostPendingContract;
            pContract.CustomerId = Guid.Parse(HttpContext.Session.GetString(SessionKey.Customer.CustomerID));
            pContract.EmployeeCreatedId = Guid.Parse(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID));

            var respone = await _pContractService.addAsnyc(pContract);
            
            if (respone != null)
            {
                string[] split = respone.Split('*');
                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "pcontract");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                var customer = await _customerService.GetCustomerById(pContract.CustomerId.ToString());
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tạo hợp đồng cho khách hàng {customer.FullName} - ID:{customer.CustomerId.ToString().Substring(0,8)}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

        

                if (IsAuthenticate == 1)
                {
                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Tạo hợp đồng thành công !!";
                    return RedirectToAction("ListCusAccount", "Admin");
                }
                else
                {
                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Tạo hợp đồng thành công !!";
                    return RedirectToAction("ListCus");
                }
            }
            TempData["SweetType"] = "error";
            TempData["SweetIcon"] = "error";
            TempData["SweetTitle"] = "Tạo hợp đồng thất bại, kiểm tra thông tin hợp đồng!!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListCus()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            List<Models.Customer> customersList = new List<Models.Customer>();
            
            customersList = await _customerService.GetAllCustomers();
            return View(customersList);
            
        }

        public async Task<IActionResult> Index()
        {
            string VB = ViewBag.Role;
            if (IsAuthenticate == 3)
            {
                VMPersonalPage vm = new VMPersonalPage();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Employee = await _employeeService.GetEmployeePutById(EmployeeId);
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
                vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                ViewData["Role"] = VB;
                return View(vm);
            }
            else if (IsAuthenticate == 0) { }
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Bạn chưa đăng nhập !!";          
            }
            return RedirectToAction("Index", "Verify");
        }

        public IActionResult AddCus()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View();
        }

        public async Task<IActionResult> CreateFormForCus(string id)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            HttpContext.Session.SetString(SessionKey.Customer.CustomerID, id);
            VMCreateFormForCus vm = new VMCreateFormForCus();
            vm.TypeOfServices = await _tosService.GetAll();
            return View(vm);
            
        }

        public async Task<IActionResult> DetailsCus(string customerID)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var respone = await _customerService.GetCustomerById(customerID);
            if (respone != null)
            {
                VMDetailsCus vm = new VMDetailsCus();
                vm.Customer = respone;
                //truyền thêm
                vm.PendingContracts = await _pContractService.getListCusId(customerID);
                vm.DoneContracts = await _dContractService.getListByCusId(customerID);
                
                if (vm.Customer.IsLocked)
                {
                    ViewBag.block = "d-none";
                }
                else
                {
                    ViewBag.block = "";
                }
              
                return View(vm);
            }
            else
            {
                  
                return RedirectToAction("ListCus", "BusinessStaff");
            }
        }

        public IActionResult DetailsDContract()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View();
        }

        public async Task<IActionResult> AddCusAction(PostCustomer customer)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            customer.IsLocked = false;
            if (customer.BuisinessName != null)
            {
                customer.typeofCustomer = "Doanh nghiệp";
            }
            else
            {
                customer.typeofCustomer = "Cá nhân";
            }
            customer.DateAdded = DateTime.Now;
            int reponse = await _customerService.AddNewCustomer(customer);

            if (reponse != 0)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 khách hàng {customer.FullName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm khách hàng thành công !!";

                if(isAuthenticate == 1)
                {
                    return RedirectToAction("ListCusAccount");
                }
                else
                {
                     return RedirectToAction("ListCus");
                }
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin khách hàng!!";
                return RedirectToAction("ListCus");
            }
        }

        //TContract
        public IActionResult ContractFormPage()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View();
        }

        public async Task<IActionResult> ListContractFormPage()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View(await _tContractService.getAllAsnyc());
        }

        public async Task<IActionResult> AddTContract(API.PostTContract tContract)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            IFormFile temp = null;
            if (tContract.File != null)
            {
                temp = tContract.File;
                if (tContract.File.ContentType.StartsWith("application/pdf"))
                {
                    if (tContract.File.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            tContract.File.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            tContract.TContractName = tContract.File.FileName.ToString().Replace(".pdf", "");
                            tContract.Base64StringFile = Convert.ToBase64String(bytes);
                            tContract.File = null;
                        }
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Tải lên file ảnh thất bại, kiểm tra lại thông tin file ảnh!!";
                    RedirectToAction("AddEmpAccount");
                }
            }
            var reponse = await _tContractService.addAsnyc(tContract);

            if (reponse != 0)
            {
                string inputFile = _uploadHelper.UploadPDF(temp, _hostingEnvironment.WebRootPath, "TempFile", "");
                _pdfToImageHelper.PdfToPng(inputFile, reponse, "tcontract");
                
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(inputFile);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 mẫu hợp đồng - {tContract.TContractName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                //thành công
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm mẫu hợp đồng thành công !!";
                return RedirectToAction("ListContractFormPage");
            }
            else
            {
                //thất bại
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm mẫu hợp đồng thất bại, kiểm tra lại thông tin mẫu hợp đồng!!";
                return RedirectToAction("ListContractFormPage");
            }
        }

        public async Task<IActionResult> EditContractFormPage(string TContactID)
        {

            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            TemplateContract template = await _tContractService.getByIdAsnyc(int.Parse(TContactID));
            HttpContext.Session.SetString("EditTContractID", TContactID);
            return View(template);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCFormPage([FromBody] PutTContract tContract)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            string TContactID = HttpContext.Session.GetString("EditTContractID");
            tContract.TContractID = int.Parse(TContactID);
            var respone = await _tContractService.updateAsnyc(tContract);
            HttpContext.Session.SetString("EditTContractID", "");
            if (respone != 0)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã chỉnh sửa cấu hình 1 mẫu hợp đồng - {tContract.TContractName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Chỉnh sửa cấu hình thành công !!";
                return RedirectToAction("ListContractFormPage");
            }
          /*  TempData["SweetType"] = "error";
            TempData["SweetIcon"] = "error";
            TempData["SweetTitle"] = "Chỉnh sửa cấu hình bị lỗi!!";*/
            else return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteTContract(int tContractId)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var tContract = _tContractService.getByIdAsnyc(tContractId);
            var respone = await _tContractService.DeleteTContract(tContractId);
            if (respone != 0)
            {
                string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath +"\\" + "TContractImage", tContractId.ToString());
                string[] filePaths = Directory.GetFiles(directoryPath);
                foreach (var item in filePaths)
                {
                    System.IO.File.Delete(item);
                }
                Directory.Delete(directoryPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã xoá 1 mẫu hợp đồng - {tContract.Result.TContractName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa mẫu hợp đồng thành công !!";
                return RedirectToAction("ListContractFormPage");
            }
            else return RedirectToAction("Index");
        }

        //TMinute

        public IActionResult MinuteFormPage()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View();
        }

        public async Task<IActionResult> ListMinuteFormPage()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            return View(await _tMinuteService.GetAll());
        }

        public async Task<IActionResult> EditMinuteFormPage(int tMinuteId)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            TemplateMinute template = await _tMinuteService.GetById(tMinuteId);
            HttpContext.Session.SetString("EditTMinuteID", tMinuteId.ToString());

            return View(template);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMFormPage([FromBody] PutTMinute tMinute)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            string TMinuteID = HttpContext.Session.GetString("EditTMinuteID");
            tMinute.TMinuteID = int.Parse(TMinuteID);
            var respone = await _tMinuteService.Update(tMinute);
            HttpContext.Session.SetString("EditTMinuteID", "");
            if (respone != 0)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã cập nhật cấu hình 1 mẫu biên bản - {tMinute.TMinuteName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật cấu hình thành công !!";
                return RedirectToAction("ListContractFormPage");
            }
            else return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddTMinute(API.PostTMinute tMinute)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            IFormFile temp = null;
            if (tMinute.File != null)
            {
                temp = tMinute.File;
                if (tMinute.File.ContentType.StartsWith("application/pdf"))
                {
                    if (tMinute.File.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            tMinute.File.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            tMinute.TMinuteName = tMinute.File.FileName.ToString().Replace(".pdf", "");
                            tMinute.Base64StringFile = Convert.ToBase64String(bytes);
                            tMinute.File = null;
                        }
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Tải lên file ảnh thất bại, kiểm tra lại thông tin file ảnh!!";
                    RedirectToAction("AddEmpAccount");
                }
            }
            var reponse = await _tMinuteService.AddNew(tMinute);

            if (reponse != 0)
            {
                string inputFile = _uploadHelper.UploadPDF(temp, _hostingEnvironment.WebRootPath, "TempFile", "");
                _pdfToImageHelper.PdfToPng(inputFile, reponse, "tminute");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(inputFile);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 mẫu biên bản - {tMinute.TMinuteName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                //thành công
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm mẫu biên bản thành công !!";
                return RedirectToAction("ListMinuteFormPage");
            }
            else
            {
                //thất bại
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm mẫu biên bản thất bại, kiểm tra lại thông tin mẫu biên bản!!";
                return RedirectToAction("AddCus");
            }
        }

        public async Task<IActionResult> DeleteTMinute(int tMinuteId)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var tMinute = _tMinuteService.GetById(tMinuteId);
            var respone = await _tMinuteService.DeleteTMinute(tMinuteId);
            if (respone != 0)
            {
                string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\" + "TMinuteImage", tMinuteId.ToString());
                string[] filePaths = Directory.GetFiles(directoryPath);
                foreach (var item in filePaths)
                {
                    System.IO.File.Delete(item);
                }
                Directory.Delete(directoryPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã xoá 1 mẫu biên bản.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa mẫu biên bản thành công !!";
                return RedirectToAction("ListMinuteFormPage");
            }
            else return RedirectToAction("Index");
        }

        //done contract
        /*public async Task<IActionResult> Contracts_Approved()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            List<DContractViewModel> contractList = new List<DContractViewModel>();

            if (IsAuthenticate == 1)
            {
                contractList = await _dContractService.getListIsEffect();
            }
            else
            {
                contractList = _dContractService.getListIsEffect().Result.Where(d => d.EmployeeCreatedId == EmployeeId).ToList();
            }
            
            return View(contractList);
            
        }*/

        // hiển thị list chờ duyệt theo role và id ng tạo
        // yeu cau lap dat theo id nguoi tao va admin
        public async Task<IActionResult> Contracts_PendingApproval()
        {
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            ViewData["Tille"] = "Yêu cầu lắp đặt";
            ViewData["ViewBack"] = "Contracts_PendingApproval";
            if (IsAuthenticate == 3  ) 
            {
                pContractList = await _pContractService.getListWaitDirSignsEmpId(EmployeeId);
                return View(pContractList);
            }
            if(IsAuthenticate == 1)
            {
                pContractList = await _pContractService.getListWaitDirectorSigns();
                return View(pContractList);
            }
            return RedirectToAction("Index", "Verify");
        }

        //danh sách từ chối duyệt theo nguoi tao va admin
        public async Task<IActionResult> ContractListRefuse()
        {
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            ViewData["Tille"] = "Hợp đồng bị từ chối";
            ViewData["ViewBack"] = "ContractListRefuse";
            if (IsAuthenticate == 3)
            {
                pContractList = await _pContractService.getListRefuseByEmpId(EmployeeId);
                return View("Contracts_PendingApproval", pContractList);
            }
            if (IsAuthenticate == 1)
            {
                pContractList = await _pContractService.getListRefuse();
                return View("Contracts_PendingApproval", pContractList);
            }
             return RedirectToAction("Index", "Verify"); 

        }

        //hiển thị danh sách chờ kh ký theo role và theo nhân viên tạo hoặc ng ký va admin
        public async Task<IActionResult> ContractListWaitSign()
        {
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            ViewData["Tille"] = "Hợp đồng chờ khách ký";
            ViewData["ViewBack"] = "ContractListWaitSign";
            if (IsAuthenticate == 3) {
                pContractList = await _pContractService.getListWaitCusSignsByEmpId(EmployeeId);
                return View("Contracts_PendingApproval", pContractList);
            }
            if (IsAuthenticate == 1) {
                pContractList = await _pContractService.getListWaitCustomerSigns();
                return View("Contracts_PendingApproval", pContractList);
            }
            
            
            return RedirectToAction("Index", "Verify");
        }

        /*public async Task<IActionResult> DetailsContractEffect(string id)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMDetailsContract viewModel = new VMDetailsContract();
            viewModel.DoneContracts = await _dContractService.getByIdAsnyc(id);
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.DoneContracts.CustomerId);
            viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.DoneContracts.DirectorSignedId);
            return View(viewModel);
            
        }*/

        public async Task<IActionResult> Details_Contract_PendingApproved(string id, string tille, string viewBack)
        {
            if (IsAuthenticate != 1 && IsAuthenticate != 3) { return RedirectToAction("Index", "Verify"); }
            VMDetailsContract viewModel = new VMDetailsContract();
            viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
            viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.EmployeeCreatedId);
            viewModel.Tille = tille;
            viewModel.ViewBack = viewBack;
            return View(viewModel);
            
        }

        /*public async Task<IActionResult> DetailsContractRefuse(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();

            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }

            viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
            viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.DirectorSignedId);
            
            return View("Details_Contract_Approved", viewModel);

        }*/

       /* public async Task<IActionResult> DetailsContractWaitSign(string id)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMDetailsContract viewModel = new VMDetailsContract();
            viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
            viewModel.Customer = await _customerService.GetCustomerById(viewModel.PendingContracts.CustomerId);
            viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.PendingContracts.EmployeeCreatedId);
            
            return View("Details_Contract_Approved", viewModel);
        }*/

        public async Task<IActionResult> EditCus(string customerID)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var customer = await _customerService.GetCustomerByIdPut(customerID);
            return View(customer);
            
        }

        public async Task<IActionResult> UpdateInfoCustomer(PutCustomer putCustomer)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
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

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin khách hàng {putCustomer.FullName}  - ID:{putCustomer.CustomerId.ToString().Substring(0,8)}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật thành công !!";

                return View("EditCus", customer);
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin khách hàng!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> HistoryOperation()
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var employee = JsonConvert.DeserializeObject<Employee>(empContext);

            var respone = await _historyEmpSvc.GetListById(employee.EmployeeId.ToString());

            foreach (var name in respone)
            {
                name.OperationName = name.OperationName.Replace($"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)}", "Bạn");
            }

            if (respone != null)
            {
                return View(respone);
            }
            else
            {
                //báo lỗi
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Không tìm thấy lịch sử nào cả !!";
                return View();
            }

        }

        public IActionResult SendMail()
        {
            return View();
        }

        public async Task<IActionResult> UpdateInfo(PutEmployee employee)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            if (employee.ImageFile != null)
            {
                var temp = employee.ImageFile;
                employee = await _employeeService.GetEmployeePutById(employee.EmployeeId.ToString());

                if (temp.ContentType.StartsWith("image/"))
                {
                    if (temp.Length > 0)
                    {
                        if (employee.Image != null)
                        {
                            _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, employee.Image));
                        }
                        string imagePath = _uploadHelper.UploadImage(temp, _hostingEnvironment.WebRootPath, "Avatars");
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Tải lên file ảnh thất bại, kiểm tra lại thông tin file ảnh!!";
                    RedirectToAction("AddEmpAccount");
                }
            }
            string respone = await _employeeService.UpdateEmployee(employee);

            if (respone != null || employee.FullName == null)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật thành công !!";

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            if (null == sData)
                return NotFound();

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            try
            {
                int? fileCount = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}")).Length;
                if (fileCount == 5)
                {
                    //đã đủ 5 ảnh trong dtb, yêu cầu xóa 1 ảnh để có thể thêm mới
                    return RedirectToAction("Index", "Verify");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid().ToString().Substring(0, 8) + ".png";

            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\SignatureImages", serialPFX);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            if (certificate.ImageSignature1 == null)
            {
                certificate.ImageSignature1 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
            }
            else if (certificate.ImageSignature2 == null)
            {
                certificate.ImageSignature2 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
            }
            else if (certificate.ImageSignature3 == null)
            {
                certificate.ImageSignature3 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
            }
            else if (certificate.ImageSignature4 == null)
            {
                certificate.ImageSignature4 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
            }
            else if (certificate.ImageSignature5 == null)
            {
                certificate.ImageSignature5 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
            }
            else
            {
                //tt hết slot chữ ký
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Đã hết lượt thêm chữ ký!!";
                return View("Index");
            }

            bmpSign.Save(filePath, ImageFormat.Png);

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tạo 1 chữ ký cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Tạo chữ ký thành công !!";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        public async Task<ActionResult> UploadSignature(VMPersonalPage vm)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            API.PFXCertificate certificate = new API.PFXCertificate();
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;

            try
            {
                int? fileCount = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}")).Length;
                if (fileCount == 5)
                {
                    //đã đủ 5 ảnh trong dtb, yêu cầu xóa 1 ảnh để có thể thêm mới
                    return RedirectToAction("Index", "Verify");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            var temp = vm.PFXCertificate.ImageFile;

            if (temp != null)
            {
                if (temp.ContentType.StartsWith("image/"))
                {
                    certificate = await _pfxCertificateServices.GetById(serialPFX);

                    string imagePath = _uploadHelper.UploadImage(temp, _hostingEnvironment.WebRootPath + "\\SignatureImages", serialPFX);
                    ;
                    if (certificate.ImageSignature1 == null)
                    {
                        certificate.ImageSignature1 = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                    else if (certificate.ImageSignature2 == null)
                    {
                        certificate.ImageSignature2 = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                    else if (certificate.ImageSignature3 == null)
                    {
                        certificate.ImageSignature3 = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                    else if (certificate.ImageSignature4 == null)
                    {
                        certificate.ImageSignature4 = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                    else if (certificate.ImageSignature5 == null)
                    {
                        certificate.ImageSignature5 = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    }
                    else
                    {
                        //tt hết slot chữ ký
                        TempData["SweetType"] = "error";
                        TempData["SweetIcon"] = "error";
                        TempData["SweetTitle"] = "Đã hết lượt thêm chữ ký!!";
                        return View("Index");
                    }
                }
            }

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tải lên 1 chữ ký cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật chữ ký thành công !!";

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveTextSignature(string imageData)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            if (imageData == null)
                return NotFound();

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            try
            {
                int? fileCount = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}")).Length;
                if (fileCount == 5)
                {
                    //đã đủ 5 ảnh trong dtb, yêu cầu xóa 1 ảnh để có thể thêm mới
                    return RedirectToAction("Index", "Verify");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            try
            {
                // Chuyển đổi base64 thành byte array
                byte[] bytes = Convert.FromBase64String(imageData.Split(',')[1]);

                // Đặt tên cho ảnh
                var fileName = Guid.NewGuid().ToString().Substring(0, 8) + ".png";

                string folderPath = Path.Combine(_hostingEnvironment.WebRootPath + "\\SignatureImages", serialPFX);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, fileName);

                if (certificate.ImageSignature1 == null)
                {
                    certificate.ImageSignature1 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                }
                else if (certificate.ImageSignature2 == null)
                {
                    certificate.ImageSignature2 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                }
                else if (certificate.ImageSignature3 == null)
                {
                    certificate.ImageSignature3 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                }
                else if (certificate.ImageSignature4 == null)
                {
                    certificate.ImageSignature4 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                }
                else if (certificate.ImageSignature5 == null)
                {
                    certificate.ImageSignature5 = filePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                }
                else
                {
                    //tt hết slot chữ ký
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Đã hết lượt thêm chữ ký!!";
                    return View("Index");
                }

                System.IO.File.WriteAllBytes(filePath, bytes);

                var result = await _pfxCertificateServices.Update(certificate);

                if (result != null)
                {
                    var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                    Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tạo 1 chữ ký cá nhân.",
                        EmployeeID = employeeDoing.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Tạo chữ ký thành công !!";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Verify");
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        public async Task<ActionResult> DeleteSignature(string filePath)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            if (filePath == certificate.ImageSignature1)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature1));
                certificate.ImageSignature1 = null;
            }
            else if (filePath == certificate.ImageSignature2)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature2));
                certificate.ImageSignature2 = null;
            }
            else if (filePath == certificate.ImageSignature3)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature3));
                certificate.ImageSignature3 = null;
            }
            else if (filePath == certificate.ImageSignature4)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature4));
                certificate.ImageSignature4 = null;
            }
            else if (filePath == certificate.ImageSignature5)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature5));
                certificate.ImageSignature5 = null;
            }
            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã xoá 1 chữ ký cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "warning";
                TempData["SweetIcon"] = "warning";
                TempData["SweetTitle"] = "Xóa chữ ký thành công !!";

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        public async Task<ActionResult> SetDefaultImageSignature(string filePath)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            certificate.DefaultImageSignature = filePath;

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                TempData["SweetType"] = "warning";
                TempData["SweetIcon"] = "warning";
                TempData["SweetTitle"] = "Đặt mặc định thành công !!";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        public async Task<ActionResult> DeleteDefaultSignature(string filePath)
        {
            if (IsAuthenticate != 3 && IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            certificate.DefaultImageSignature = null;

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa chữ ký mặc định thành công !!";
                return RedirectToAction("Index");
            }
            else
            {

                return RedirectToAction("Index", "Verify");
            }
        }
    }
}