using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using Spire.Pdf.Graphics;
using System.Drawing.Imaging;
using test.Models;
using QuanLyHopDongVaKySo.CLIENT.Helpers;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class DirectorController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleService _roleService;
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IPFXCertificateServices _pfxCertificateServices;
        private readonly IUploadHelper _uploadHelper;

        private int isAuthenticate;
        private string employeeId;
       
        public DirectorController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IRoleService roleService,
            IPositionService positionSerivce, IEmployeeService employeeService, IPFXCertificateServices pfxCertificateServices,
            IUploadHelper uploadHelper)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _roleService = roleService;
            _positionService = positionSerivce;
            _pfxCertificateServices = pfxCertificateServices;
            _employeeService = employeeService;
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
        public async Task<IActionResult> Index()
        {
            string VB = ViewBag.Role;
            if (IsAuthenticate == 2)
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
                return RedirectToAction("Index","Verify");
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
                return RedirectToAction("Index","Verify");
            }
        }
        public IActionResult ListContractActive()
        {
            return View();
        }
        public IActionResult HistoryOperation()
        {
            return View();
        }
        public IActionResult InforSign()
        {
            return View();
        }
        public IActionResult DetailsContractAwait()
        {
            return View();
        }
        public IActionResult DetailsApprovedContract()
        {
            return View();
        }
        public IActionResult DetailsActiveContract()
        {
            return View();
        }
        public IActionResult ListContractEffect()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {
            if (null == sData)
                return NotFound();

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);
            int fileCount = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}")).Length;

            if (fileCount == 5)
            {
                //đã đủ 5 ảnh trong dtb, yêu cầu xóa 1 ảnh để có thể thêm mới
                return RedirectToAction("Index", "Verify");
            }
            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid().ToString().Substring(0, 8) + ".png";

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

            int fileCount = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, $"SignatureImages/{serialPFX}")).Length;

            if (fileCount == 5)
            {
                //đã đủ 5 ảnh trong dtb, yêu cầu xóa 1 ảnh để có thể thêm mới
                return RedirectToAction("Index", "Verify");
            }
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
    }
}
