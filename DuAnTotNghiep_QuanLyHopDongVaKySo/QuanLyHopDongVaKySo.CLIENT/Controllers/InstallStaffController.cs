using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.SigningServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using VMAPI = QuanLyHopDongVaKySo_API.ViewModels;
using Syncfusion.EJ2.Maps;
using System.Drawing.Imaging;
using test.Models;
using API = QuanLyHopDongVaKySo_API.Models; 
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class InstallStaffController : Controller
    {
        private readonly IIRequirementService _iRequirementService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleService _roleService;
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IPFXCertificateServices _pfxCertificateServices;
        private readonly IDContractsService _doneContractSvc;
        private readonly IPMinuteService _pMinuteService;
        private readonly ITOSService _tosService;
        private readonly ITMinuteService _tMinuteService;
        private readonly IInstallationDevicesService _installationDevicesService;
        private readonly ITContractService _tContractService;
        private readonly IUploadHelper _uploadHelper;
        private readonly IPasswordService _passwordService;
        private readonly ICustomerService _customerService;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly ISigningService _signingService;
        private readonly IHistoryEmpSvc _historyEmpSvc;
        private readonly IHistoryCusSvc _historyCusSvc;
        private readonly IDMinuteService _dMinuteService;


        private int isAuthenticate;
        private string employeeId;

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

        public InstallStaffController(IIRequirementService iRequirementService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IRoleService roleService,
            IPositionService positionSerivce, IEmployeeService employeeService, IPFXCertificateServices pfxCertificateServices, IDContractsService doneContractSvc,
            IPMinuteService pMinuteService, ITOSService tosService, ITMinuteService tMinuteService, IInstallationDevicesService installationDevicesService, ITContractService tContractService,
            IUploadHelper uploadHelper, IPasswordService passwordService, IPdfToImageHelper pdfToImageHelper, ICustomerService customerService, ISigningService signingService,
            IHistoryEmpSvc historyEmpSvc, IHistoryCusSvc historyCusSvc, IDMinuteService dMinuteService)
        {
            _iRequirementService = iRequirementService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _roleService = roleService;
            _positionService = positionSerivce;
            _pfxCertificateServices = pfxCertificateServices;
            _employeeService = employeeService;
            _doneContractSvc = doneContractSvc;
            _pMinuteService = pMinuteService;
            _tosService = tosService;
            _tMinuteService = tMinuteService;
            _installationDevicesService = installationDevicesService;
            _tContractService = tContractService;
            _uploadHelper = uploadHelper;
            _passwordService = passwordService;
            _pdfToImageHelper = pdfToImageHelper;
            _customerService = customerService;
            _signingService = signingService;
            _historyEmpSvc = historyEmpSvc;
            _historyCusSvc = historyCusSvc;
            _dMinuteService = dMinuteService;
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
        public async Task<IActionResult> ChangePassAction([FromBody] VMAPI.ChangePassword change)
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
        public async Task<IActionResult> Record_Install_Pendding()
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                VMListInstallRecord vm = new VMListInstallRecord()
                {
                    pendingMinutes = await _pMinuteService.GetByEmpId(EmployeeId),
                };
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
        }
        public async Task<IActionResult> Record_Install_Require()
        {
            VMListIRequire vm = new VMListIRequire();
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                vm.IRequirements = await _iRequirementService.GetAll();
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");

        }

        public async Task<IActionResult> GetTaskFormIR(int iRequirementID)
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                PostGetTaskFromIR task = new PostGetTaskFromIR()
                {
                    IRequirementId = iRequirementID,
                    EmployeeID = HttpContext.Session.GetString(SessionKey.Employee.EmployeeID)
                };
                var respone = await _pMinuteService.GetTaskFormIRequirement(task);
                if (respone != null)
                {
                    string[] split = respone.Split('*');
                    string pdfPath = null;
                    IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                    pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                    _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "pminute");

                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(pdfPath);

                    var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                    Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã nhận 1 yêu cầu lắp đặt - ID Biên Bản: {split[1]}.",
                        EmployeeID = employeeDoing.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);

                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Đã nhận yêu cầu lắp đặt !!";
                    return RedirectToAction("Record_Install_Pendding");
                }
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Đã có lỗi xảy ra !!";
                return RedirectToAction("Record_Install_Pendding");
            }
            return RedirectToAction("Index", "Verify");
        }

        public async Task<IActionResult> DetailsRequire(string id)
        {
            VMDeitalsMinute vm = new VMDeitalsMinute();

            vm.Requirement = _iRequirementService.GetAll().Result.Where(r => r.InstallRequireID == int.Parse(id)).FirstOrDefault();
            vm.DoneContract = await _doneContractSvc.getByIdAsnyc(vm.Requirement.DoneContractId.ToString());
            vm.Customer = await _customerService.GetCustomerById(vm.DoneContract.CustomerId.ToString());
            //vm.Employee = await _employeeService.GetEmployeeById(vm.PendingMinute.EmployeeId.ToString());
            return View(vm);
        }

        public async Task<IActionResult> DetailsInstallComplete(string id)
        {
            VMDeitalsMinute vm = new VMDeitalsMinute();

            vm.DoneMinute = await _dMinuteService.GetById(int.Parse(id));
            //vm.DoneContract = _doneContractSvc.getAllView().Result.FirstOrDefault(d => d.DMinuteID == id);
            //vm.Customer = await _customerService.GetCustomerById(vm.DoneContract.CustomerId.ToString());
            vm.Employee = await _employeeService.GetEmployeeById(EmployeeId);
            return View(vm);
        }

        public async Task<IActionResult> DetailsInstallAwait(string id)
        {
            VMDeitalsMinute vm = new VMDeitalsMinute();

            vm.PendingMinute = await _pMinuteService.GetById(int.Parse(id));
            vm.DoneContract = await _doneContractSvc.getByIdAsnyc(vm.PendingMinute.DoneContractId.ToString());
            vm.Customer = await _customerService.GetCustomerById(vm.DoneContract.CustomerId.ToString());
            vm.Employee = await _employeeService.GetEmployeeById(vm.PendingMinute.EmployeeId.ToString());
            return View(vm);
        }
    
        public async Task<IActionResult> Index()
        {
            if (IsAuthenticate == 4)
            {
                VMPersonalPage vm = new VMPersonalPage();
                vm.Positions = await _positionService.GetAllPositionsAsync();
                vm.Roles = await _roleService.GetAllRolesAsync();
                vm.Employee = await _employeeService.GetEmployeePutById(EmployeeId);
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
                vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                return View(vm);
            }
                return RedirectToAction("Index", "Verify");
            
        }

        public async Task<IActionResult> UpdateInfo(PutEmployee employee)
        {
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

      

        public async Task<IActionResult> Record_Install_Complete()
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                VMListInstallRecord vm = new VMListInstallRecord()
                {
                    doneMinutes = await _dMinuteService.GetListByEmpId(EmployeeId)
                };
                return View(vm);
            }
             return RedirectToAction("Index", "Verify");
        }

        public async Task<IActionResult> SignByCus(int pMinuteID)
        {
            VMSignByStaff vm = new VMSignByStaff();
            try
            {
                vm.PMinute = await _pMinuteService.GetById(pMinuteID);
                var dContract = _doneContractSvc.getAll().Result.FirstOrDefault(d => d.DContractID == vm.PMinute.DoneContractId);
                vm.Customer = await _customerService.GetCustomerById(dContract.CustomerId.ToString());
                vm.PFXCertificate = await _pfxCertificateServices.GetById(vm.Customer.SerialPFX);

                HttpContext.Session.SetString(SessionKey.PedningMinute.PMinuteID, vm.PMinute.PendingMinuteId.ToString());
                HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, vm.Customer.SerialPFX);
                HttpContext.Session.SetString(SessionKey.Customer.CustomerID, dContract.CustomerId.ToString());

            }
            catch (Exception)
            {
                //báo lỗi
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Khách hàng ký thất bại, kiểm tra thông tin ký của khách hàng!!";
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        public async Task<IActionResult> SignByStaff(int pMinuteID)
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            
                VMSignByStaff vm = new VMSignByStaff();
                try
                {
                    vm.PMinute = await _pMinuteService.GetById(pMinuteID);
                    var dContract = _doneContractSvc.getAll().Result.FirstOrDefault(d => d.DContractID == vm.PMinute.DoneContractId);
                    vm.Customer = await _customerService.GetCustomerById(dContract.CustomerId.ToString());
                    vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                    string tosName = _tosService.GetById(dContract.TOS_ID).Result.ServiceName;
                    ViewBag.DContracID = dContract.DContractID;
                    ViewBag.ServiceName = tosName;

                    if (vm.PMinute.IsIntallation)
                    {
                        ViewBag.block = "d-none";
                    }
                    else
                    {
                        ViewBag.block = "";
                    }

                }
                catch (Exception)
                {
                    //báo lỗi
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Nhân viên ký thất bại, kiểm tra lại thông tin ký của nhân viên!!";
                }
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
        }

        [HttpPost]
        public async Task<IActionResult> SignContract([FromBody] VMAPI.SigningModel signing)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, signing.ImagePath));
            signing.Base64StringFile = Convert.ToBase64String(fileBytes);
            signing.ImagePath = null;
            try
            {
                string stampPath = Path.Combine(_hostingEnvironment.WebRootPath, "StampImage");

                if (!Directory.Exists(stampPath))
                {
                    Directory.CreateDirectory(stampPath);
                }

                string fileStamp = Directory.GetFiles(stampPath)[0];
                if (fileStamp == null)
                {
                    //báo lỗi ko có ảnh mộc

                    return RedirectToAction("Index", "Verify");
                }
                else
                {
                    byte[] fileStampBytes = System.IO.File.ReadAllBytes(fileStamp);
                    signing.Base64StringFileStamp = Convert.ToBase64String(fileStampBytes);
                }
            }
            catch (Exception)
            {
                //báo lỗi ko có mộc
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Không có ảnh mộc nào cả!!";
                return BadRequest();
            }

            var respone = await _signingService.SignMinuteByInstaller(signing);


            if (respone != null)
            {
                string[] split = respone.Split('*');
                string pdfPath = null;

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                string folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PMinuteImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[1]);

                string[] imageFiles = Directory.GetFiles(folderItem);

                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);

                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "pminute");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã ký biên bản lắp đặt- ID Biên Bản: {split[1]}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Ký chữ ký thành công !!";
                return RedirectToAction("Record_Install_Complete");
            }
            else
            {
                //báo lỗi ký có nhiều cái tui sẽ nói chỉ sau
          /*      TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Ký quá nhiều lần !!";*/
                return RedirectToAction("Record_Install_Complete");
            }

           
        }


        [HttpPost]
        public async Task<ActionResult> CustomerSign([FromBody] SignData sData)
        {

            string pMinuteID = HttpContext.Session.GetString(SessionKey.PedningMinute.PMinuteID);
            string serial = HttpContext.Session.GetString(SessionKey.PFXCertificate.Serial);

            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid().ToString().Substring(0, 8) + ".png";

            string folderPath = Path.Combine(_hostingEnvironment.WebRootPath, "TempSignature");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);


            bmpSign.Save(filePath, ImageFormat.Png);


            VMAPI.SigningModel signing = new VMAPI.SigningModel();
            signing.IdFile = pMinuteID;
            signing.Serial = serial;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            signing.Base64StringFile = Convert.ToBase64String(fileBytes);

            var respone = await _signingService.SignMinuteByCustomer(signing);

            if (respone != null)
            {
                //0: base. 1: dminute id, 2: pminuteid
                string[] split = respone.Split('*');
                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "minute");

                //xoa anh pcontract
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PMinuteImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[2]);

                string[] imageFiles = Directory.GetFiles(folderItem);

                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                Customer customerDoing = await _customerService.GetCustomerById(HttpContext.Session.GetString(SessionKey.Customer.CustomerID));
                API.OperationHistoryCus historyCus = new API.OperationHistoryCus()
                {
                    OperationName = $"{customerDoing.FullName} đã ký biên bản - ID:{pMinuteID}.",
                    CustomerID = customerDoing.CustomerId
                };
                await _historyCusSvc.AddNew(historyCus);
            }
            else
            {
                //báo lỗi
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Ký không thành công!!";
                return BadRequest();
            }

            HttpContext.Session.SetString(SessionKey.PedningMinute.PMinuteID, "");
            HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, "");
            HttpContext.Session.SetString(SessionKey.Customer.CustomerID,"");

            _uploadHelper.RemoveImage(filePath);

            //xem thông tin hiển thị alert ở CusToSign hàm SaveSign(), vì dùng http nên không có trả ở return nó sẽ chạy hết hàm SaveSign(). Các action khác mà được gọi từ ajax
            //cũng hiển thị thông báo tương tự
            return Ok();
        }

        public async Task<IActionResult> HistoryOperation()
        {
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var employee = JsonConvert.DeserializeObject<Employee>(empContext);

            var respone = await _historyEmpSvc.GetListById(employee.EmployeeId.ToString());

            foreach (var name in respone)
            {
                name.OperationName = name.OperationName.Replace($"{employee.FullName} - ID:{employee.EmployeeId.ToString().Substring(0,8)}", "Bạn");
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
                TempData["SweetTitle"] = "Không có lịch sử thao tác nào cả!!";
                return View();
            }

        }


        public async Task<IActionResult> ListTypeOfService()
        {
            VMListTOS vm = new VMListTOS()
            {
                TypeOfServices = await _tosService.GetAll(),
                TemplateMinutes = await _tMinuteService.GetAll(),
                TemplateContracts = await _tContractService.getAllAsnyc()
            };
            return View(vm);
        }

        public async Task<IActionResult> DetailsTypeOfService(int tosID)
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                VMDetailsTypeOfService vm = new VMDetailsTypeOfService()
                {
                    InstallationDevices = await _installationDevicesService.GetAllByServiceId(tosID),
                    InstallationDevice = new API.InstallationDevice()
                };
                HttpContext.Session.SetString("tosID", tosID.ToString());
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
        }

        public async Task<IActionResult> AddDeviceAction(VMDetailsTypeOfService vm)
        {
            if (IsAuthenticate == 1 || IsAuthenticate == 4)
            {
                API.InstallationDevice device = new API.InstallationDevice();
                device = vm.InstallationDevice;
                var respone = await _installationDevicesService.AddNewDevice(device);
                if (respone != null)
                {
                    var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                    Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{employee.FullName} đã thêm thiết bị {device.DeviceName} vào dịch vụ {_tosService.GetAll().Result.FirstOrDefault(s => s.TOS_ID == device.TOS_ID).ServiceName}.",
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
                    TempData["SweetTitle"] = "Thêm thất bại, kiểm tra lại thông tin thiết bị và số lượng !!!!";
                    return RedirectToAction("DetailsTypeOfService", new { tosID = device.TOS_ID });
                }
            }
            return RedirectToAction("Index", "Verify");
        }

        [HttpPut]
        public async Task<IActionResult> EditDeviceAction([FromBody] API.InstallationDevice device)
        {
            var respone = await _installationDevicesService.UpdateDevice(device);
            if (respone != null)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employee = JsonConvert.DeserializeObject<Employee>(empContext);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employee.FullName} đã cập nhật thông tin thiết bị {device.DeviceName} của dịch vụ {_tosService.GetAll().Result.FirstOrDefault(s => s.TOS_ID == device.TOS_ID).ServiceName}.",
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
                return RedirectToAction("DetailsTypeOfService", device.TOS_ID);
            }
        }

        public async Task<IActionResult> DelDeviceAction(int deviceID)
        {
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
                    OperationName = $"{employee.FullName} đã xoá thiết bị {deviceName.DeviceName} khỏi dịch vụ {serviceName}.",
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
                TempData["SweetTitle"] = "Xóa thất bại, kiểm tra thông tin cần xóa!!";
                return RedirectToAction("ListRole");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {
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
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0,8)} đã tạo 1 chữ ký cá nhân.",
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
                TempData["SweetTitle"] = "Tải lên chữ ký thành công !!";
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

        public async Task<ActionResult> SetDefaultImageSignature(string filePath)
        {
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
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            certificate.DefaultImageSignature = null;

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {

                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Xóa chữ  mặt định thành công !!";
                return RedirectToAction("Index");
            }
            else
            {

                return RedirectToAction("Index", "Verify");
            }
        }
    }
}