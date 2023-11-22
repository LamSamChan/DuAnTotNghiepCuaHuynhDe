﻿using Microsoft.AspNetCore.Mvc;
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
            IUploadHelper uploadHelper, IPasswordService passwordService, IPdfToImageHelper pdfToImageHelper, ICustomerService customerService, ISigningService signingService)
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
                return BadRequest();
            }
        }

        public async Task<IActionResult> ListInstallRequire()
        {
            VMListIRequire vm = new VMListIRequire()
            {
                IRequirements = await _iRequirementService.GetAll(),
                DContracts = await _doneContractSvc.getAll(),
            };
            return View(vm);
        }

        public async Task<IActionResult> GetTaskFormIR(int iRequirementID)
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
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "minute");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                return RedirectToAction("ListInstallRecord");
            }
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }

        public IActionResult DetailsRequire()
        {
            return View();
        }

        public IActionResult ListDevice()
        {
            return View();
        }

        public IActionResult DetailsDevice()
        {
            return View();
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
            else
            {
                return RedirectToAction("Index", "Verify");
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
                        employee.Image = imagePath.Replace(_hostingEnvironment.WebRootPath + @"\", "");
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

        public IActionResult DetailsInstall()
        {
            return View();
        }

        public async Task<IActionResult> ListInstallRecord()
        {
            VMListIRequire vm = new VMListIRequire()
            {
                PMinutes = _pMinuteService.GetAll().Result.Where(e => e.EmployeeId.ToString() == EmployeeId).ToList(),
                DContracts = await _doneContractSvc.getAll()
            };
            return View(vm);
        }

        public IActionResult SignByCus()
        {
            return View();
        }

        public async Task<IActionResult> SignByStaff(int pMinuteID)
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
            }
            catch (Exception)
            {
                //báo lỗi
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> SignContract([FromBody] VMAPI.SigningModel signing)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, signing.ImagePath));
            signing.Base64StringFile = Convert.ToBase64String(fileBytes);
            signing.ImagePath = null;
            try
            {
                string fileStamp = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "StampImage"))[0];
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
                return BadRequest();
            }

            var respone = await _signingService.SignMinuteByInstaller(signing);


            if (respone != null)
            {
                string[] split = respone.Split('*');
                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "minute");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);
            }

            return RedirectToAction("ListInstallRecord");
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
            VMDetailsTypeOfService vm = new VMDetailsTypeOfService()
            {
                InstallationDevices = await _installationDevicesService.GetAllByServiceId(tosID)
            };
            HttpContext.Session.SetString("tosID", tosID.ToString());
            return View(vm);
        }

        public async Task<IActionResult> AddDeviceAction(VMDetailsTypeOfService vm)
        {
            API.InstallationDevice device = new API.InstallationDevice();
            device = vm.InstallationDevice;
            var respone = await _installationDevicesService.AddNewDevice(device);
            if (respone != null)
            {
                TempData["SwalMessageType"] = "success";
                TempData["SwalMessageIcon"] = "success";
                TempData["SwalMessageTitle"] = "Thêm dịch vụ thành công !!";
                return RedirectToAction("DetailsTypeOfService", new { tosID = device.TOS_ID });
            }
            else
            {
                TempData["SwalMessageType"] = "error";
                TempData["SwalMessageIcon"] = "error";
                TempData["SwalMessageTitle"] = "Xảy ra lỗi!!";
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
                certificate.ImageSignature1 = filePath.Replace(_hostingEnvironment.WebRootPath + @"/", "");
            }
            else if (certificate.ImageSignature2 == null)
            {
                certificate.ImageSignature2 = filePath.Replace(_hostingEnvironment.WebRootPath + @"/", "");
            }
            else if (certificate.ImageSignature3 == null)
            {
                certificate.ImageSignature3 = filePath.Replace(_hostingEnvironment.WebRootPath + @"/", "");
            }
            else if (certificate.ImageSignature4 == null)
            {
                certificate.ImageSignature4 = filePath.Replace(_hostingEnvironment.WebRootPath + @"/", "");
            }
            else if (certificate.ImageSignature5 == null)
            {
                certificate.ImageSignature5 = filePath.Replace(_hostingEnvironment.WebRootPath + @"/", "");
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

        public async Task<ActionResult> DeleteDefaultSignature(string filePath)
        {
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

            certificate.DefaultImageSignature = null;

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