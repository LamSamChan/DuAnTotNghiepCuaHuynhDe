using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.InstallationDevicesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TMinuteServices;
using QuanLyHopDongVaKySo.CLIENT.Services.TOSServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Models;
using System.Drawing.Imaging;
using test.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using Employee = QuanLyHopDongVaKySo_API.Models.Employee;
using static QuanLyHopDongVaKySo.CLIENT.Constants.SessionKey;
using System.Net.Http.Json;

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



        private int isAuthenticate = 1;
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

        public BusinessStaffController(ICustomerService customerService, IDContractsService dContractService, IEmployeeService employeeService,
            IPContractService pContractService, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, ITContractService tContractService,
            ITOSService tosService, ITMinuteService tMinuteService, IInstallationDevicesService installationDevicesService, IPFXCertificateServices pfxCertificateServices,
            IRoleService roleService, IPositionService positionSerivce, IUploadHelper uploadHelper, IPdfToImageHelper pdfToImageHelper)
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

        }
        public async Task<IActionResult> CreatePContract(PostPendingContract pContract)
        {
            pContract.CustomerId = Guid.Parse(HttpContext.Session.GetString(SessionKey.Customer.CustomerID));
            pContract.EmployeeCreatedId = Guid.Parse(HttpContext.Session.GetString(SessionKey.Employee.EmployeeID));
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ListCus()
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
            else
            {
                return RedirectToAction("Index", "Verify");
            }
        }
        public IActionResult AddCus()
        {
            return View();
        }
        public async Task<IActionResult> CreateFormForCus(string id)
        {
            HttpContext.Session.SetString(SessionKey.Customer.CustomerID, id);
            VMCreateFormForCus vm = new VMCreateFormForCus();
            vm.TypeOfServices = await _tosService.GetAll();

            return View(vm);
        }
        public async Task<IActionResult> DetailsCus(string customerID)
        {
            var respone = await _customerService.GetCustomerById(customerID);
            if (respone != null)
            {
                VMDetailsCus vm = new VMDetailsCus();
                vm.Customer = respone;

                //truyền thêm
                vm.PendingContracts = await _pContractService.getListCusId(customerID); ;
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
        public IActionResult MinuteFormPage()
        {
            return View();
        }
        public IActionResult ChangePass()
        {
            return View();
        }
        public async Task<IActionResult> ListContractFormPage()
        {

            return View(await _tContractService.getAllAsnyc());

        }
        public async Task<IActionResult> EditContractFormPage(string TContactID)
        {
            
            TemplateContract template = await _tContractService.getByIdAsnyc(int.Parse(TContactID));
            return View(template);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCFormPage(PutTContract tContract)
        {
           
                await _tContractService.updateAsnyc(tContract);
                return RedirectToAction("ListContractFormPage", "BusinessStaff");
            
            
        }

        public async Task<IActionResult> TListMinute()
        {

            return View(await _tMinuteService.GetAll());

        }

        public async Task<IActionResult> TEditMinute(int TMinuteID)
        {

            TemplateMinute template = await _tMinuteService.GetById(TMinuteID);
            return View(template);

        }

        [HttpPost]
        public async Task<IActionResult> TUpdateMinute(PutTMinute tContract)
        {

            await _tMinuteService.Update(tContract);
            return RedirectToAction("TListMinute", "BusinessStaff");


        }

        /*public IActionResult EditContratFormPage()
        {
            return View();
        }*/
        public IActionResult TEditMinute()
        {
            return View();
        }

        public async Task<IActionResult> AddTMinute([FromForm] API.PostTMinute tContract)
        {
            if (tContract.File != null)
            {
                if (tContract.File.ContentType.StartsWith("application/pdf"))
                {
                    if (tContract.File.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            tContract.File.CopyTo(stream);
                            byte[] bytes = stream.ToArray();
                            tContract.TMinuteName = tContract.File.FileName.ToString().Replace(".pdf", "");
                            tContract.Base64StringFile = Convert.ToBase64String(bytes);
                            tContract.File = null;
                        }
                    }
                }
                else
                {
                    //báo lỗi ko tải lên file ảnh
                    RedirectToAction("AddEmpAccount");
                }
            }
            var reponse = await _tMinuteService.AddNew(tContract);

            if (reponse != 0)
            {
                //thành công
                return RedirectToAction("MinutuFormPage");
            }
            else
            {
                //thất bại
                return RedirectToAction("AddCus");
            }
        }

        public async Task<IActionResult> AddTContract(API.PostTContract tContract)
        {
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
                    RedirectToAction("AddEmpAccount");
                }
            }
            var reponse = await _tContractService.addAsnyc(tContract);

            if (reponse != 0)
            {
                string inputFile = _uploadHelper.UploadPDF(temp, _hostingEnvironment.WebRootPath, "TempFile");
                _pdfToImageHelper.PdfToPng(inputFile, reponse, "tcontract");
                FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
                fs.Close();
                System.IO.File.Delete(inputFile);

                //thành công
                return RedirectToAction("ContractFormPage");
            }
            else
            {
                //thất bại
                return RedirectToAction("AddCus");
            }
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
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            if (isAuthenticate == 1)
            {
                pContractList = await _pContractService.getListWaitDirectorSigns();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = await _pContractService.getListWaitDirSignsEmpId(employeeId);
                return View(pContractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(pContractList);
            }
        }


        //danh sách từ chối duyệt
        public async Task<IActionResult> ContractListRefuse()
        {
            List<PContractViewModel> pContractList = new List<PContractViewModel>();
            if (isAuthenticate == 1 || isAuthenticate == 2)
            {
                pContractList = await _pContractService.getListRefuse();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = await _pContractService.getListRefuseByEmpId(employeeId);
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
                pContractList = await _pContractService.getListWaitCustomerSigns();
                return View(pContractList);
            }
            else if (isAuthenticate == 3)
            {
                pContractList = await _pContractService.getListWaitCusSignsByEmpId(employeeId);
                return View(pContractList);
            }
            else if (isAuthenticate == 2)
            {
                pContractList = await _pContractService.getListWaitCusSignsByDirId(employeeId);
                return View(pContractList);
            }
            else
            {
                //người khác role và vô thì trả về index của họ, sửa lại khi đủ FE
                return View(pContractList);
            }
        }

        public async Task<IActionResult> DetailsContractEffect(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();
            try
            {
                viewModel.DoneContracts = await _dContractService.getByIdAsnyc(id);
                viewModel.Customer = await _customerService.GetCustomerById(viewModel.DoneContracts.CustomerId);
                viewModel.Employee = await _employeeService.GetEmployeeById(viewModel.DoneContracts.DirectorSignedId);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        public async Task<IActionResult> DetailsContractPending(string id)
        {
            VMDetailsContract viewModel = new VMDetailsContract();
            try
            {
                viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
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
                viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
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
                viewModel.PendingContracts = await _pContractService.getByIdAsnyc(id);
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

        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {
            if (null == sData)
                return NotFound();

            var empContext = HttpContext.Session.GetString(SessionKey.Employee.EmployeeContext);
            var serialPFX = JsonConvert.DeserializeObject<Employee>(empContext).SerialPFX;
            var certificate = await _pfxCertificateServices.GetById(serialPFX);

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