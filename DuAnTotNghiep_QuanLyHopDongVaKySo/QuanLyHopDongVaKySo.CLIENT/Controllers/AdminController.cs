using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Models;
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
using QuanLyHopDongVaKySo.CLIENT.Services.StampService;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.Drawing.Imaging;
using test.Models;
using API = QuanLyHopDongVaKySo_API.Models;
using APIPost = QuanLyHopDongVaKySo_API.Models.ViewPost;
using APITPut = QuanLyHopDongVaKySo_API.Models.ViewPuts;
using PutEmployee = QuanLyHopDongVaKySo.CLIENT.Models.ModelPut.PutEmployee;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly ICustomerService _customerService;
        private readonly IPFXCertificateServices _pfxCertificateServices;
        private readonly IPContractService _pContractService;
        private readonly IDContractsService _doneContractSvc;
        private readonly ITOSService _tosService;
        private readonly ITContractService _tContractService;
        private readonly ITMinuteService _tMinuteService;
        private readonly IInstallationDevicesService _installationDevicesService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUploadHelper _uploadHelper;
        private readonly IStampSvc _stampSvc;
        private readonly IHistoryEmpSvc _historyEmpSvc;
        private readonly IHistoryCusSvc _historyCusSvc;
        private readonly IPasswordService _passwordService;


        private int isAuthenticate;
        private string employeeId;

        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService,
            ICustomerService customerService, IPFXCertificateServices pfxCertificateServices, IPContractService pContractService,
            IDContractsService doneContractSvc, ITOSService tosService, ITContractService tContractService, ITMinuteService tMinuteService,
            IInstallationDevicesService installationDevicesService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor,
            IUploadHelper uploadHelper, IStampSvc stampSvc, IHistoryEmpSvc historyEmpSvc, IPasswordService passwordService, IHistoryCusSvc historyCusSvc)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
            _customerService = customerService;
            _pfxCertificateServices = pfxCertificateServices;
            _pContractService = pContractService;
            _doneContractSvc = doneContractSvc;
            _tosService = tosService;
            _tContractService = tContractService;
            _tMinuteService = tMinuteService;
            _installationDevicesService = installationDevicesService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _uploadHelper = uploadHelper;
            _stampSvc = stampSvc;
            _historyEmpSvc = historyEmpSvc;
            _passwordService = passwordService;
            _historyCusSvc = historyCusSvc;

        }

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

        public IActionResult ChangePass()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        public async Task<IActionResult> Index1()
        {
            List<Models.Customer> customersList = new List<Models.Customer>();

            customersList = await _customerService.GetAllCustomers();
            return View(customersList);

        }
        public async Task<IActionResult> HistoryOperation()
        {
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var employee = JsonConvert.DeserializeObject<Employee>(empContext);

            var respone = await _historyEmpSvc.GetListById(employee.EmployeeId.ToString());

            List<API.OperationHistoryEmp> historyEmps = new List<API.OperationHistoryEmp>();
            historyEmps = await _historyEmpSvc.GetAll();

            foreach (var item in historyEmps)
            {
                item.OperationName = item.OperationName.Replace($"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)}", "Bạn");
            }

            VMHistoryAdmin vm = new VMHistoryAdmin()
            {
                HistoryCus = await _historyCusSvc.GetAll(),
                HistoryEmps = historyEmps
            };
            return View(vm);
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

                    TempData["SwalType"] = "success";
                    TempData["SwalIcon"] = "success";
                    TempData["SwalTitle"] = $"{employeeDoing.FullName} hay đổi mật khẩu thành công !!";
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

        public async Task<IActionResult> Index()
        {
            
            string VB = ViewBag.Role;
            if (IsAuthenticate == 1)
            {
                VMAdminIndex vm = new VMAdminIndex();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Employee = await _employeeService.GetEmployeePutById(EmployeeId);
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
                vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                vm.Stamps = await _stampSvc.GetAll();
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

        public async Task<IActionResult> ListTypeOfService()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMListTOS vm = new VMListTOS();
            
            vm.TypeOfServices = await _tosService.GetAll();
            vm.TemplateMinutes = await _tMinuteService.GetAll();
            vm.TemplateContracts = await _tContractService.getAllAsnyc();
            return View(vm);
            
        }

        public async Task<IActionResult> AddTOSAction(VMListTOS vm)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            APIPost.PostTOS tos = new APIPost.PostTOS();
            tos = vm.PostTOS;
            var repone = await _tosService.AddNew(tos);
            if (repone != 0)
            {

                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 dịch vụ :{tos.ServiceName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm dịch vụ thành công !!";
                return RedirectToAction("ListTypeOfService");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin dịch vụ!!!!";
                return RedirectToAction("ListRole");
            }
        }

        public async Task<IActionResult> UpdateTOSAction(VMListTOS vm)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            APITPut.PutTOS tos = new APITPut.PutTOS();
            tos = vm.PutTOS;
            var repone = await _tosService.Update(tos);
            if (repone != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin dịch vụ {tos.ServiceName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật dịch vụ thành công !!";
                return RedirectToAction("ListTypeOfService");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin dịch vụ!!";
                return RedirectToAction("ListRole");
            }
        }

        public async Task<IActionResult> DetailsTypeOfService(int tosID)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMDetailsTypeOfService vm = new VMDetailsTypeOfService();
            
            vm.InstallationDevices = await _installationDevicesService.GetAllByServiceId(tosID);
            vm.InstallationDevice = new API.InstallationDevice();
            HttpContext.Session.SetString("tosID", tosID.ToString());
            return View(vm);
        }

        public async Task<IActionResult> AddDeviceAction(VMDetailsTypeOfService vm)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            API.InstallationDevice device = new API.InstallationDevice();
            device = vm.InstallationDevice;
            var respone = await _installationDevicesService.AddNewDevice(device);
            if (respone != null)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã thêm thiết bị {device.DeviceName} vào dịch vụ {_tosService.GetAll().Result.FirstOrDefault(s => s.TOS_ID == device.TOS_ID).ServiceName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm thiết bị thành công !!";
                return RedirectToAction("DetailsTypeOfService", new { tosID = device.TOS_ID });
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin thiết bị!!";
                return RedirectToAction("ListRole");
            }
        }

        public async Task<IActionResult> DelDeviceAction(int deviceID)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var deviceName = await _installationDevicesService.GetDeviceById(deviceID);
            string serviceName = _tosService.GetAll().Result.FirstOrDefault(s => s.TOS_ID == deviceName.TOS_ID).ServiceName;

            var respone = await _installationDevicesService.DelectDevice(deviceID);
            string tosID = HttpContext.Session.GetString("tosID");
            if (respone != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã xoá thiết bị {deviceName.DeviceName} khỏi dịch vụ {serviceName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa thiết bị thành công !!";
                return RedirectToAction("DetailsTypeOfService", new { tosID = tosID });
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Xóa thiết bị thất bại, kiểm tra lại thông tin thiết bị!!";
                return RedirectToAction("ListRole");
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditDeviceAction([FromBody] API.InstallationDevice device)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var respone = await _installationDevicesService.UpdateDevice(device);
            if (respone != null)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin thiết bị {device.DeviceName} của dịch vụ {_tosService.GetAll().Result.FirstOrDefault(s => s.TOS_ID == device.TOS_ID).ServiceName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật thiết bị thành công !!";
                return RedirectToAction("DetailsTypeOfService", device.TOS_ID);
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin thiết bị!!";
                return RedirectToAction("ListRole");
            }
        }

        public async Task<IActionResult> EditTypeOfService(int tosID)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMListTOS vm = new VMListTOS();
            
            vm.PutTOS = await _tosService.GetByPutId(tosID);
            vm.TemplateMinutes = await _tMinuteService.GetAll();
            vm.TemplateContracts = await _tContractService.getAllAsnyc();
            return View(vm);
            
        }

        

        public async Task<IActionResult> ListPFXCertificate()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMListPFX vm = new VMListPFX();
            
            vm.Customers = await _customerService.GetAllCustomers();
            vm.Employees = await _employeeService.GetAllEmployees();
            vm.PFXCertificates = await _pfxCertificateServices.GetAll();
            vm.PFXCertificatesE = await _pfxCertificateServices.GetAllExpire();
            vm.PFXCertificatesATE = await _pfxCertificateServices.GetAllAboutToExpire();
            return View(vm);
            
        }

        public async Task<IActionResult> DetailsPFXCertificate(string serial)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var respone = await _pfxCertificateServices.GetById(serial);
                return View(respone);
            
        }

        public async Task<IActionResult> UpdateNotAfterPFX(string serial)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var respone = await _pfxCertificateServices.UpdateNotAfter(serial);
            if (respone != null)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã gia hạn thời gian hiệu lực của chứng chỉ số PFX {serial}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                //update thành công
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Gia hạn thành công !!";
                return RedirectToAction("ListPFXCertificate");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Gia hạn thất bại, kiểm tra lại thông tin gia hạn!!";
                return RedirectToAction("DetailsPFXCertificate");
            }
        }

        public async Task<IActionResult> DetailsEmpAccount(string empId)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMDetailsEmpAccount vm = new VMDetailsEmpAccount();
            if(IsAuthenticate == 1)
            {
                vm.Employee = await _employeeService.GetEmployeeById(empId);
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                //truyền thêm pcontract + donecontract
                vm.PendingContracts = await _pContractService.getListEmpId(empId); //getAllAsnyc().Result.Where(p => p.EmployeeCreatedId == empId).ToList();
                vm.DoneContracts =await _doneContractSvc.getListByEmpId(empId); //getAllView().Result.Where(d => d.EmployeeCreatedId == empId).ToList();
                return View(vm);
            };
            return RedirectToAction("Index", "Verify");
        }

        public async Task<IActionResult> EditEmpAccount(string empId)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMEditEmployee vm = new VMEditEmployee();
            
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();
            vm.Employee = await _employeeService.GetEmployeePutById(empId);
            return View(vm);
            
        }

        public async Task<IActionResult> EditEmpAction(PutEmployee employee)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var existEmp = await _employeeService.GetEmployeeById(employee.EmployeeId.ToString());
            if (employee.ImageFile != null)
            {
                if (employee.ImageFile.ContentType.StartsWith("image/"))
                {
                    if (employee.ImageFile.Length > 0)
                    {
                        if (existEmp.Image != null)
                        {
                            _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, existEmp.Image));
                        }
                        string imagePath = _uploadHelper.UploadImage(employee.ImageFile, _hostingEnvironment.WebRootPath, "Avatars");
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                        employee.ImageFile = null;
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
            var emp = await _employeeService.GetEmployeePutById(respone);

            if (emp != null)
            {
                VMEditEmployee vm = new VMEditEmployee();
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Employee = emp;

                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin của nhân viên {employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0,8)}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật nhân viên thành công !!";
                return View("EditEmpAccount", vm);
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin nhân viên!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddEmpAccount()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMAddEmployee vm = new VMAddEmployee();
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();
            return View(vm);
            
        }

        public async Task<IActionResult> AddEmpAction(PostEmployee employee)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            if (employee.ImageFile != null)
            {
                if (employee.ImageFile.ContentType.StartsWith("image/"))
                {
                    if (employee.ImageFile.Length > 0)
                    {
                        string imagePath = _uploadHelper.UploadImage(employee.ImageFile, _hostingEnvironment.WebRootPath, "Avatars");
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath, "");
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

            var reponse = await _employeeService.AddNewEmployee(employee);

            if (reponse != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 nhân viên mới - {employee.FullName}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm nhân viên thành công !!";
                return RedirectToAction("ListEmpAccount");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin nhân viên!!";
                return RedirectToAction("ListEmpAccount");
            }
        }

        public async Task<IActionResult> ListEmpAccount()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMAdminListUsersAccount vm = new VMAdminListUsersAccount();
            
            vm.Employees = await _employeeService.GetAllEmployees();
            vm.Positions = await _positionService.GetAllPositionsAsync();
            vm.Roles = await _roleService.GetAllRolesAsync();
            return View(vm);
            
        }

        public async Task<IActionResult> ListCusAccount()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMAdminListUsersAccount vm = new VMAdminListUsersAccount();
             
            vm.Customers = await _customerService.GetAllCustomers();
            return View(vm);
            
        }

        public async Task<IActionResult> ListPosition()
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            VMListPosition vm = new VMListPosition();
            
            vm.Positions = await _positionService.GetAllPositionsAsync();
            vm.Employees = await _employeeService.GetAllEmployees();
            return View(vm);
            
        }

        public async Task<IActionResult> ListRole()
        
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }

            VMListRole vm = new VMListRole();
            
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Employees = await _employeeService.GetAllEmployees();
            return View(vm);
            
        }

        public async Task<IActionResult> AddPosition(API.Position position)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            int reponse = await _positionService.AddPositionAsync(position);
            if (reponse != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} đã thêm 1 chức vụ mới - {position.PositionName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm chức danh thành công !!";
                return RedirectToAction("ListPosition");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin chức danh!!!!";
                return RedirectToAction("ListPosition");
            }
        }

        public async Task<IActionResult> EditPosition(int positionId)
        {
            if (IsAuthenticate != 1){ return RedirectToAction("Index", "Verify"); }
            
            var position = await _positionService.GetPositionByIdAsync(positionId);
                if (position != null)
                {
                    return View(position);
                }
                else
                {
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin chức danh!!";
                    return RedirectToAction("Index");
                }
            
        }

        public async Task<IActionResult> UpdatePosition(API.Position position)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var update = await _positionService.UpdatePositionAsync(position);
            if (update != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} đã cập nhật thông tin chức vụ {position.PositionName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);


                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật chức danh thành công !!";
                return RedirectToAction("ListPosition");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin chức danh!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddRole(API.Role role)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            int reponse = await _roleService.AddRoleAsync(role);
            if (reponse != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã thêm 1 vai trò mới - {role.RoleName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm vai trò thành công !!";
                return RedirectToAction("ListRole");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin vai trò!!";
                return RedirectToAction("ListRole");
            }
        }

        public async Task<IActionResult> EditRole(int roleId)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var role = await _roleService.GetRoleByIdAsync(roleId);
            if (role != null)
            {
                return View(role);
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin vai trò!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UpdateRole(API.Role role)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var update = await _roleService.UpdateRoleAsync(role);
            if (update != 0)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0, 8)} đã cập nhật thông tin vai trò {role.RoleName}.",
                    EmployeeID = employee.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật vai trò thành công !!";
                return RedirectToAction("ListRole");
            }
            else
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Cập nhật thất bại, kiểm tra lại thông tin vai trò!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UpdateInfo(PutEmployee employee)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
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
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã cập nhật lại thông tin của bản thân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Cập nhật bản thân thành công !!";
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
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
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

        public async Task<ActionResult> UploadSignature(VMAdminIndex vm)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            API.PFXCertificate certificate = new API.PFXCertificate();
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;

            var temp = vm.PFXCertificate.ImageFile;

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

            if (temp != null)
            {
                if (temp.ContentType.StartsWith("image/"))
                {
                    certificate = await _pfxCertificateServices.GetById(serialPFX);

                    string imagePath = _uploadHelper.UploadImage(temp, _hostingEnvironment.WebRootPath + "\\SignatureImages", serialPFX);

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
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
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


        public async Task<ActionResult> UploadStampImage(VMAdminIndex vm)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var temp = vm.Stamp.ImageFile;
            if (temp != null)
            {
                if (temp.ContentType.StartsWith("image/"))
                {
                    string imagePath = _uploadHelper.UploadImage(temp, _hostingEnvironment.WebRootPath, "StampImage");
                    API.Stamp stamp = new API.Stamp();
                    stamp.StampPath = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
                    var result = await _stampSvc.AddNew(stamp);
                    if (result != 0)
                    {
                        var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                        Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                        API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                        {
                            OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tải lên ảnh dấu mộc công ty.",
                            EmployeeID = employeeDoing.EmployeeId
                        };
                        await _historyEmpSvc.AddNew(historyEmp);

                        TempData["SweetType"] = "success";
                        TempData["SweetIcon"] = "success";
                        TempData["SweetTitle"] = "Lưu mộc đóng dấu thành công !!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, imagePath));
                        TempData["SweetType"] = "error";
                        TempData["SweetIcon"] = "error";
                        TempData["SweetTitle"] = "Hiện tại đang có dấu mộc tồn tại hãy xoá để có thể tải ảnh mộc mới!!";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    // ko phải file ảnh
                    TempData["SweetType"] = "warning";
                    TempData["SweetIcon"] = "warning";
                    TempData["SweetTitle"] = "Hãy chọn file ảnh!!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                // chưa up ảnh
                TempData["SweetType"] = "warning";
                TempData["SweetIcon"] = "warning";
                TempData["SweetTitle"] = "Bạn chưa tải ảnh lên!!";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> DeleteSignature(string filePath)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            if (filePath == certificate.ImageSignature1)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature1));

                if (certificate.DefaultImageSignature == certificate.ImageSignature1)
                {
                    certificate.DefaultImageSignature = null;
                }

                certificate.ImageSignature1 = null;
            }
            else if (filePath == certificate.ImageSignature2)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature2));

                if (certificate.DefaultImageSignature == certificate.ImageSignature2)
                {
                    certificate.DefaultImageSignature = null;
                }

                certificate.ImageSignature2 = null;
            }
            else if (filePath == certificate.ImageSignature3)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature3));

                if (certificate.DefaultImageSignature == certificate.ImageSignature3)
                {
                    certificate.DefaultImageSignature = null;
                }

                certificate.ImageSignature3 = null;
            }
            else if (filePath == certificate.ImageSignature4)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature4));

                if (certificate.DefaultImageSignature == certificate.ImageSignature4)
                {
                    certificate.DefaultImageSignature = null;
                }

                certificate.ImageSignature4 = null;
            }
            else if (filePath == certificate.ImageSignature5)
            {
                _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, certificate.ImageSignature5));

                if (certificate.DefaultImageSignature == certificate.ImageSignature5)
                {
                    certificate.DefaultImageSignature = null;
                }

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

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa chữ ký thành công !!";
                return RedirectToAction("Index");
            }
            else
            {
               
                return RedirectToAction("Index", "Verify");
            }
        }

        public async Task<ActionResult> DeleteStampImage(string filePath)
        {
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var stamp = _stampSvc.GetAll().Result.FirstOrDefault(s => s.StampPath == filePath);

            _uploadHelper.RemoveImage(Path.Combine(_hostingEnvironment.WebRootPath, filePath));
            var respone = await _stampSvc.Delete(stamp.ID);

            if (respone != 0)
            {
                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tải xoá ảnh dấu mộc công ty.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa mộc đóng dấu thành công !!";
                return RedirectToAction("Index");
            }
            else
            {
               
                return RedirectToAction("Index", "Verify");
            }
        }

        public async Task<ActionResult> SetDefaultImageSignature(string filePath)
        {

            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            certificate.DefaultImageSignature = filePath;

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
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
            if (IsAuthenticate != 1) { return RedirectToAction("Index", "Verify"); }
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