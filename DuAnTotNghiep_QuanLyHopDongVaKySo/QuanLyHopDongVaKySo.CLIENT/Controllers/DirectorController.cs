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
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using VMAPI = QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.SigningServices;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Services.DMinuteServices;
using static iTextSharp.text.pdf.AcroFields;

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
        private readonly IPasswordService _passwordService;
        private readonly IDContractsService _dContractService;
        private readonly IPContractService _pContractService;
        private readonly ICustomerService _customerService;
        private readonly ISigningService _signingService;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly IHistoryEmpSvc _historyEmpSvc;
        private readonly IDMinuteService _dMinuteService;
        private int isAuthenticate;
        private string employeeId;
        public DirectorController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IRoleService roleService,
            IPositionService positionSerivce, IEmployeeService employeeService, IPFXCertificateServices pfxCertificateServices,
            IUploadHelper uploadHelper, IPasswordService passwordService, IDContractsService dContractsService, IPContractService pContractService,
            ICustomerService customerService, ISigningService signingService, IPdfToImageHelper pdfToImageHelper, IHistoryEmpSvc historyEmpSvc, IDMinuteService dMinuteService)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _roleService = roleService;
            _positionService = positionSerivce;
            _pfxCertificateServices = pfxCertificateServices;
            _employeeService = employeeService;
            _uploadHelper = uploadHelper;
            _passwordService = passwordService;
            _dContractService = dContractsService;
            _pContractService = pContractService;
            _customerService = customerService;
            _signingService = signingService;
            _pdfToImageHelper = pdfToImageHelper;
            _historyEmpSvc = historyEmpSvc;
            _dMinuteService = dMinuteService;
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
            else if(IsAuthenticate == 0)
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Bạn chưa đăng nhập !!";
            }
            return RedirectToAction("Index", "Verify");
        }

        public IActionResult ChangePass()
        {
            return View();
        }
        
        public async Task<IActionResult> Contracts_Approved()
        {
            if (IsAuthenticate == 2 || IsAuthenticate == 1)
            {
                List<VMAPI.PContractViewModel> pContractList = new List<VMAPI.PContractViewModel>();
                if (IsAuthenticate == 2)
                {
                    pContractList = await _pContractService.getListDirSignsEmpId(EmployeeId);
                }
                if (IsAuthenticate == 1)
                {
                    pContractList = await _pContractService.getListWaitCustomerSigns();
                }
                return View(pContractList);
            }
            return RedirectToAction("Index", "Verify");
        }
        public async Task<IActionResult> Details_Contract_Approved(string id)
        {
            if (IsAuthenticate == 2 || IsAuthenticate == 1) {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;

                VMDetailsContractAwait vm = new VMDetailsContractAwait();
                try
                {
                    vm.PContract = await _pContractService.getByIdAsnyc(id);
                    vm.EmployeeCreated = await _employeeService.GetEmployeeById(vm.PContract.EmployeeCreatedId);
                    vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                    vm.Customer = await _customerService.GetCustomerById(vm.PContract.CustomerId);
                }
                catch
                {
                    //báo lỗi
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Không tìm thấy hợp đồng !!";
                    return RedirectToAction("Index");
                }
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
            
        }
        public async Task<IActionResult> Contracts_Rejected()
        {
            List<VMAPI.PContractViewModel> pContractList = new List<VMAPI.PContractViewModel>();
            if (IsAuthenticate == 2 || IsAuthenticate == 1)
            {
                pContractList = await _pContractService.getListRefuse();
                return View(pContractList);
            }
            return RedirectToAction("Index", "Verify");
        }
        public async Task<IActionResult> Details_Contract_Rejected(string id)
        {
            if (IsAuthenticate == 2 || IsAuthenticate == 1) {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;

                VMDetailsContractAwait vm = new VMDetailsContractAwait();
                try
                {
                    vm.PContract = await _pContractService.getByIdAsnyc(id);
                    vm.EmployeeCreated = await _employeeService.GetEmployeeById(vm.PContract.EmployeeCreatedId);
                    vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                    vm.Customer = await _customerService.GetCustomerById(vm.PContract.CustomerId);
                }
                catch
                {
                    //báo lỗi
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Không tìm thấy hợp đồng !!";
                    return RedirectToAction("Index");
                }
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
            
        }
        public async Task<IActionResult> Contracts_Active()
        {
            VMContractsActive vm = new VMContractsActive();

            if (IsAuthenticate == 3)
            {
                var contractList = await _dContractService.getListByEmpId(EmployeeId);

                foreach (var item in contractList)
                {
                    if (item.Status == "Đang hiệu lực")
                    {
                        vm.DContractsInEffect.Add(item);
                    }
                    else
                    {
                        vm.DContractsNotInEffect.Add(item);
                    }
                }

                return View(vm);
            }
            if(IsAuthenticate == 1 || IsAuthenticate == 2)
            {
                var contractList = await _dContractService.getAllView();

                foreach(var item in contractList)
                {
                    if (item.Status == "Đang hiệu lực")
                    {
                        vm.DContractsInEffect.Add(item);
                    }
                    else
                    {
                        vm.DContractsNotInEffect.Add(item);
                    }
                }

                return View(vm);
            }
             return RedirectToAction("Index","Verify");
        }
        public async Task<IActionResult> Details_Contract_Active(string Id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();
            try
            {
                viewModel.DoneContracts = await _dContractService.getByIdAsnyc(Id);
                viewModel.Customer = await _customerService.GetCustomerById(viewModel.DoneContracts.CustomerId);
                viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.DoneContracts.DirectorSignedId);
                if(viewModel.DoneContracts.DMinuteID != null)
                {
                    viewModel.DoneMinutes = await _dMinuteService.GetById(int.Parse(viewModel.DoneContracts.DMinuteID));
                }
                
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Contracts_PendingApproval()
        {
            if (IsAuthenticate == 2 || IsAuthenticate == 1)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
                VMListContractAwait vm = new VMListContractAwait();
            
                vm.PContracts = await _pContractService.getListWaitDirectorSigns();
                vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                return View(vm);
            }
            return RedirectToAction("Index", "Verify");
        }
        public async Task<IActionResult> Details_Contract_PendingApproval(string pContractId)
        {
            if (IsAuthenticate == 2 || IsAuthenticate == 1)
            {
                var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;

                VMDetailsContractAwait vm = new VMDetailsContractAwait();
                try
                {
                    vm.PContract = await _pContractService.getByIdAsnyc(pContractId);
                    vm.EmployeeCreated = await _employeeService.GetEmployeeById(vm.PContract.EmployeeCreatedId);
                    vm.PFXCertificate = await _pfxCertificateServices.GetById(serialPFX);
                    vm.Customer = await _customerService.GetCustomerById(vm.PContract.CustomerId);

                    if (vm.PContract.IsDirector == "Đã ký")
                    {
                        ViewBag.hide = "d-none";
                    }
                    else
                    {
                        ViewBag.hide = "";
                    }
                   

                    return View(vm);
                }
                catch
                {
                    //báo lỗi
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Không tìm thấy hợp đồng !!";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index", "Verify");
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
                if(respone1 != null)
                {
                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Thay đổi mật khẩu thành công !!";
                    var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                    Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                    API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                    {
                        OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã thay đổi mật khẩu cá nhân.",
                        EmployeeID = employeeDoing.EmployeeId
                    };
                    await _historyEmpSvc.AddNew(historyEmp);
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
        public async Task<IActionResult> RefuseContract(VMDetailsContractAwait vm)
        {
             if (IsAuthenticate == 2 || IsAuthenticate == 1)
            {
                var pContract = await _pContractService.getByIdAsnyc(vm.PContract.PContractID);

                API.PutPendingContract putPendingContract = new API.PutPendingContract()
                {
                    PContractId = int.Parse(pContract.PContractID),
                    IsRefuse = true,
                    Reason = vm.PContract.Reason,
                };
                var respone = _pContractService.updateAsnyc(putPendingContract);
                if (respone != null)
                {
                    TempData["SweetType"] = "success";
                    TempData["SweetIcon"] = "success";
                    TempData["SweetTitle"] = "Từ chối duyệt hợp đồng thành công !!";
                    return RedirectToAction("Contracts_PendingApproval");
                }
                else
                {
                    //báo lỗi
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Từ chối duyệt hợp đồng thất bại, kiểm tra lại thông tin hợp đồng!!";
                    return RedirectToAction("Contracts_PendingApproval");
                }
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

                return RedirectToAction("Index","Verify");
            }
        }

        public async Task<IActionResult> HistoryOperation()
        {
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
                return View();
            }
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
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Không tồn tại ảnh mộc!!";
                    return View("Details_Contract_PendingApproval", signing.IdFile);
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
                TempData["SweetTitle"] = "Không tồn tại ảnh mộc!!";
                return View("Details_Contract_PendingApproval", signing.IdFile);
            }

            var respone = await _signingService.SignContractByDirector(signing);

             if (respone != null)
             {
                string[] split = respone.Split('*');

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                string folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PContractImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[1]);

                string[] imageFiles = Directory.GetFiles(folderItem);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);

                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "pcontract");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã ký duyệt hợp đồng - ID: {signing.IdFile}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);
                return RedirectToAction("Contracts_PendingApproval");
            }
            else
            {
                //báo lỗi ko có mộc
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Duyệt hợp đồng thất bại!!";
                return RedirectToAction("Details_Contract_PendingApproval", signing.IdFile);
            }
        }

        private async Task<int> SignForList([FromBody] VMAPI.SigningModel signing)
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
                    return 1;
                }
                else
                {
                    byte[] fileStampBytes = System.IO.File.ReadAllBytes(fileStamp);
                    signing.Base64StringFileStamp = Convert.ToBase64String(fileStampBytes);
                }
            }
            catch (Exception)
            {
                return 2;
            }

            var respone = await _signingService.SignContractByDirector(signing);



            if (respone != null)
            {
                string[] split = respone.Split('*');

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                string folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PContractImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[1]);

                string[] imageFiles = Directory.GetFiles(folderItem);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);
                string pdfPath = null;

                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "pcontract");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.IO.File.Delete(pdfPath);

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã ký duyệt hợp đồng - ID: {signing.IdFile}.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);
                return 0;
            }
            else
            {

                return 3;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SignListContract([FromBody] List<VMAPI.SigningModel> signing)
        {
            if (signing == null)
            {
                TempData["SweetType"] = "error";
                TempData["SweetIcon"] = "error";
                TempData["SweetTitle"] = "Không có hợp đồng được chọn!!";
                return RedirectToAction("Contracts_PendingApproval");
            }
            foreach (var item in signing)
            {
                int respone = await SignForList(item);
                if (respone == 1 || respone == 2)
                {
                    //báo lỗi ko có ảnh mộc
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Không có ảnh mộc nào cả!!";
                    return RedirectToAction("Contracts_PendingApproval");
                }else if (respone == 3)
                {
                    //báo lỗi ko có mộc
                    TempData["SweetType"] = "error";
                    TempData["SweetIcon"] = "error";
                    TempData["SweetTitle"] = "Ký thất bại, kiểm tra lại thông tin ký!!";
                    return RedirectToAction("Contracts_PendingApproval");
                }
            }
            TempData["SweetType"] = "success";
            TempData["SweetIcon"] = "success";
            TempData["SweetTitle"] = "Ký hợp đồng thành công!!";
            return RedirectToAction("Contracts_PendingApproval");
        }



        public async Task<IActionResult> UnEffectContract(int Id)
        {
            var respone = await _dContractService.UnEffectContract(Id);
            if (respone != null)
            {
                return RedirectToAction("Contracts_Active");
            }
            else
            {
                //lỗi
                return RedirectToAction("Index");
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
                TempData["SweetType"] = "success";
                TempData["SweetIcon"] = "success";
                TempData["SweetTitle"] = "Thêm chữ ký thành công !!";

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã tải lên 1 chữ ký cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);


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
                TempData["SweetType"] = "warning";
                TempData["SweetIcon"] = "warning";
                TempData["SweetTitle"] = "Xóa chữ ký thành công !!";

                var empContextDoing = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
                Employee employeeDoing = JsonConvert.DeserializeObject<Employee>(empContextDoing);
                API.OperationHistoryEmp historyEmp = new API.OperationHistoryEmp()
                {
                    OperationName = $"{employeeDoing.FullName} - ID:{employeeDoing.EmployeeId.ToString().Substring(0, 8)} đã xoá 1 chữ ký cá nhân.",
                    EmployeeID = employeeDoing.EmployeeId
                };
                await _historyEmpSvc.AddNew(historyEmp);

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
    }
}
