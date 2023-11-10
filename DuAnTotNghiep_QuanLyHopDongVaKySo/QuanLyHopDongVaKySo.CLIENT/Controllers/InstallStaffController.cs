using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.IRequirementsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_CLIENT.Constants;
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
            IPMinuteService pMinuteService)
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
        }

        public async Task<IActionResult> ListInstallRequire()
        {
            VMListIRequire vm = new VMListIRequire()
            {
                IRequirements = await _iRequirementService.GetAll(),
                DContracts = await _doneContractSvc.GetAll(),
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
            var repone = await _pMinuteService.GetTaskFormIRequirement(task);
            if (repone != 0)
            {
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
                        using (var stream = new MemoryStream())
                        {
                            temp.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            employee.Base64String = Convert.ToBase64String(bytes);
                            employee.ImageFile = null;
                        }
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
                DContracts = await _doneContractSvc.GetAll()
            };
            return View(vm);
        }
        public IActionResult SignByCus()
        {
            return View();

        }
        public IActionResult SignByStaff()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {
            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid() + ".png";

            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "TempSignatures"), fileName);

            bmpSign.Save(filePath, ImageFormat.Png);

            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            string base64String = Convert.ToBase64String(bytes);

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var result = await _pfxCertificateServices.UploadSignatureImage(serialPFX, base64String);


            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(filePath);

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
            API.PFXCertificate pfx = new API.PFXCertificate();
            pfx = vm.PFXCertificate;
            if (pfx.ImageFile != null)
            {
                if (pfx.ImageFile.ContentType.StartsWith("image/"))
                {
                    using (var stream = new MemoryStream())
                    {
                        pfx.ImageFile.CopyTo(stream);
                        byte[] bytes = stream.ToArray();
                        pfx.Base64StringFile = Convert.ToBase64String(bytes);
                        pfx.ImageFile = null;
                    }
                }
            }
            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var result = await _pfxCertificateServices.UploadSignatureImage(serialPFX, pfx.Base64StringFile);
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
            var result = await _pfxCertificateServices.DeleteImage(serialPFX, filePath);
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
