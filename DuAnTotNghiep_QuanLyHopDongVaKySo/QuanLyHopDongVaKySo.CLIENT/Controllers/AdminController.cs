using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Org.BouncyCastle.Ocsp;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using APIPost = QuanLyHopDongVaKySo_API.Models.ViewPost;
using APITPut = QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Models;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using test.Models;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using static iTextSharp.text.pdf.AcroFields;
using Org.BouncyCastle.Crypto.Tls;

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


        private int isAuthenticate;
        private string employeeId;

        public AdminController(IPositionService positionService, IEmployeeService employeeService, IRoleService roleService,
            ICustomerService customerService, IPFXCertificateServices pfxCertificateServices, IPContractService pContractService,
            IDContractsService doneContractSvc, ITOSService tosService, ITContractService tContractService, ITMinuteService tMinuteService,
            IInstallationDevicesService installationDevicesService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor,
            IUploadHelper uploadHelper)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _roleService = roleService;
            _customerService = customerService;
            _pfxCertificateServices = pfxCertificateServices;
            _pContractService= pContractService;
            _doneContractSvc = doneContractSvc;
            _tosService = tosService;
            _tContractService = tContractService;
            _tMinuteService = tMinuteService; 
            _installationDevicesService = installationDevicesService;
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _uploadHelper = uploadHelper;
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
        public async Task<IActionResult> Index()
        {
            string VB = ViewBag.Role;
            if (IsAuthenticate == 1)
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
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }
        public async Task<IActionResult> ListTypeOfService()
        {
            VMListTOS vm = new VMListTOS() {
                TypeOfServices = await _tosService.GetAll(),
                TemplateMinutes = await _tMinuteService.GetAll(),
                TemplateContracts = await _tContractService.getAllAsnyc()
            };
            return View(vm);
        }
        public async Task<IActionResult> AddTOSAction(VMListTOS vm)
        {
            APIPost.PostTOS tos = new APIPost.PostTOS();
            tos = vm.PostTOS;
            var repone = await _tosService.AddNew(tos);
            if (repone != 0)
            {
                return RedirectToAction("ListTypeOfService");
            }
            else
            {
                return RedirectToAction("ListRole");
            }
            
        }

        public async Task<IActionResult> UpdateTOSAction(VMListTOS vm)
        {
            APITPut.PutTOS tos = new APITPut.PutTOS();
            tos = vm.PutTOS;
            var repone = await _tosService.Update(tos);
            if (repone != 0)
            {
                return RedirectToAction("ListTypeOfService");
            }
            else
            {
                return RedirectToAction("ListRole");
            }

        }

        public async Task<IActionResult> DetailsTypeOfService(int tosID)
        {
            VMDetailsTypeOfService vm = new VMDetailsTypeOfService() {
            InstallationDevices = await _installationDevicesService.GetAllByServiceId(tosID)};
            HttpContext.Session.SetString("tosID",tosID.ToString());
            return View(vm);
        }

        public async Task<IActionResult> AddDeviceAction(VMDetailsTypeOfService vm)
        {
            API.InstallationDevice device = new API.InstallationDevice();
            device = vm.InstallationDevice;
            var respone = await _installationDevicesService.AddNewDevice(device);
            if (respone != null)
            {
                return RedirectToAction("DetailsTypeOfService", new { tosID = device.TOS_ID });
            }
            else
            {
                return RedirectToAction("ListRole");
            }  
        }

        public async Task<IActionResult> DelDeviceAction(int deviceID)
        {
            var respone = await _installationDevicesService.DelectDevice(deviceID);
            string tosID = HttpContext.Session.GetString("tosID");
            if (respone != 0)
            {
                return RedirectToAction("DetailsTypeOfService", new { tosID = tosID });
            }
            else
            {
                return RedirectToAction("ListRole");
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditDeviceAction([FromBody] API.InstallationDevice device)
        {
            var respone = await _installationDevicesService.UpdateDevice(device);
            if (respone != null)
            {
                return RedirectToAction("DetailsTypeOfService", device.TOS_ID);
            }
            else
            {
                return RedirectToAction("ListRole");
            }
        }
        public async Task<IActionResult> EditTypeOfService(int tosID)
        {
            VMListTOS vm = new VMListTOS()
            {
                PutTOS = await _tosService.GetByPutId(tosID),
                TemplateMinutes = await _tMinuteService.GetAll(),
                TemplateContracts = await _tContractService.getAllAsnyc()
            };
            return View(vm);
        }
        public IActionResult ListDevice()
        {
            return View();
        }

        public async Task<IActionResult> ListPFXCertificate()
        {
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
            var respone = await _pfxCertificateServices.GetById(serial);
            return View(respone);
        }
     
        public async Task<IActionResult> UpdateNotAfterPFX(string serial)
        {
            var respone = await _pfxCertificateServices.UpdateNotAfter(serial);
            if (respone != null)
            {
                //update thành công
                return RedirectToAction("ListPFXCertificate");

            }
            else
            {
                return RedirectToAction("ListPFXCertificate");
            }
        }
        public IActionResult DetailsPosition()
        {
            return View();
        }
        public IActionResult DetailsRole()
        {
            return View();
        }
        public async Task<IActionResult> DetailsEmpAccount(string empId)
        {
            VMDetailsEmpAccount vm = new VMDetailsEmpAccount()
            {
                Employee = await _employeeService.GetEmployeeById(empId),
                Roles = await _roleService.GetAllRolesAsync(),
                Positions = await _positionService.GetAllPositionsAsync(),
                //truyền thêm pcontract + donecontract
                PendingContracts = _pContractService.getAllAsnyc().Result.Where(p => p.EmployeeCreatedId== empId).ToList(),
                DoneContracts = _doneContractSvc.getAllView().Result.Where(d => d.EmployeeCreatedId == empId).ToList(),

            }; 
            return View(vm);
        }
        public async Task<IActionResult> EditEmpAccount(string empId)
        {
            VMEditEmployee vm = new VMEditEmployee();
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();
            vm.Employee= await _employeeService.GetEmployeePutById(empId);

            if (vm.Employee != null)
            {
                return View(vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> EditEmpAction(PutEmployee employee)
        {
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
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath+ @"\", "");
                        employee.ImageFile = null;
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
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
                return View("EditEmpAccount", vm);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> AddEmpAccount()
        {
            VMAddEmployee vm = new VMAddEmployee();
            vm.Roles = await _roleService.GetAllRolesAsync();
            vm.Positions = await _positionService.GetAllPositionsAsync();

            return View(vm);
        }

        public async Task<IActionResult> AddEmpAction(PostEmployee employee)
        {
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
                    RedirectToAction("AddEmpAccount");
                }
            }
           
            var reponse = await _employeeService.AddNewEmployee(employee);
            
            if (reponse != 0)
            {
                return RedirectToAction("ListEmpAccount");
            }
            else
            {
                return RedirectToAction("AddEmpAccount");
            }
        }

        public async Task<IActionResult> ListEmpAccount()
        {
            VMAdminListUsersAccount vm = new VMAdminListUsersAccount();
            try
            {
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

        public async Task<IActionResult> ListCusAccount()
        {
            VMAdminListUsersAccount vm = new VMAdminListUsersAccount();
            try
            {
                vm.Customers = await _customerService.GetAllCustomers();
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

        public async Task<IActionResult> AddPosition(API.Position position)
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
        public async Task<IActionResult> UpdatePosition(API.Position position)
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

        public async Task<IActionResult> AddRole(API.Role role)
        {

            int reponse = await _roleService.AddRoleAsync(role);
            if (reponse != 0)
            {
                TempData["SwalMessageType"] = "success";
                TempData["SwalMessageIcon"] = "success";
                TempData["SwalMessageTitle"] = "Thêm thành công !!";
                return RedirectToAction("ListRole");
            }
            else
            {
                return RedirectToAction("ListRole");
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
        public async Task<IActionResult> UpdateRole(API.Role role)
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
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath+@"\", "");
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    RedirectToAction("AddEmpAccount");
                }
            }
            string respone = await _employeeService.UpdateEmployee(employee);

            if (respone != null || employee.FullName == null)
            {
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
            if (null == sData)
                return NotFound();

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid().ToString().Substring(0,8) + ".png";

            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}"), fileName);

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
                return View("Index");
            }

            bmpSign.Save(filePath, ImageFormat.Png);

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
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

            var temp = vm.PFXCertificate.ImageFile;

            if (temp != null)
            {
                if (temp.ContentType.StartsWith("image/"))
                {
                    
                    certificate = await _pfxCertificateServices.GetById(serialPFX);
                    
                    string imagePath = _uploadHelper.UploadImage(temp, _hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}");
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
                        return View("Index");
                    }
                }
            }

            var result = await _pfxCertificateServices.Update(certificate);

            if (result != null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
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
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

    }
}
