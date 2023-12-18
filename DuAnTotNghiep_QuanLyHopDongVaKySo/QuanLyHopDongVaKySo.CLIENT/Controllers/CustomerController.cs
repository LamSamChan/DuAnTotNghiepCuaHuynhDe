using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.EmployeesServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PositionServices;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.Services.SigningServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using VMAPI = QuanLyHopDongVaKySo_API.ViewModels;
using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using test.Models;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPFXCertificateServices _pfxCertificateServices;
        private readonly IPContractService _pContractService;
        private readonly ICustomerService _customerService;
        private readonly ISigningService _signingService;
        private readonly IUploadHelper _uploadHelper;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly IDContractsService _dContractsService;
        private readonly IHistoryCusSvc _historyCusSvc;

        private int isAuthenticate;
        private string employeeId;

        public CustomerController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IPContractService pContractService,
            ICustomerService customerService, ISigningService signingService, IPFXCertificateServices pfxCertificateServices, IUploadHelper uploadHelper
            , IPdfToImageHelper pdfToImageHelper, IDContractsService dContractsService, IHistoryCusSvc historyCusSvc)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _pContractService = pContractService;
            _customerService = customerService;
            _signingService = signingService;
            _pContractService = pContractService;
            _pfxCertificateServices = pfxCertificateServices;
            _uploadHelper = uploadHelper;
            _pdfToImageHelper = pdfToImageHelper;
            _dContractsService = dContractsService;
            _historyCusSvc = historyCusSvc;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CusToSign(string token)
        {
            HttpContext.Session.SetString(SessionKey.Customer.CustomerToken, token);
            int pContractId = DecodeToken(token);

            VMDetailsContractAwait vm = new VMDetailsContractAwait();
            try
            {
                vm.PContract = await _pContractService.getByIdAsnyc(pContractId.ToString());
                vm.Customer = await _customerService.GetCustomerById(vm.PContract.CustomerId);
                vm.PFXCertificate = await _pfxCertificateServices.GetById(vm.Customer.SerialPFX);

                HttpContext.Session.SetString(SessionKey.PedningContract.PContractID, pContractId.ToString());
                HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, vm.Customer.SerialPFX);
                HttpContext.Session.SetString(SessionKey.Customer.CustomerID, vm.PContract.CustomerId);

            }
            catch
            {
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        private int DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var pcontractId = int.Parse(tokenS.Claims.First(claim => claim.Type == "ContractID").Value);
            return pcontractId;
        }

        public IActionResult Log()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SaveSignature([FromBody] SignData sData)
        {

            string pContractID = HttpContext.Session.GetString(SessionKey.PedningContract.PContractID);
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
            signing.IdFile = pContractID;
            signing.Serial = serial;

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            signing.Base64StringFile = Convert.ToBase64String(fileBytes);

            var respone = await _signingService.SignContractByCustomer(signing);

            string idDContract = null;

            if (respone != null)
            {
                string[] split = respone.Split('*');
                idDContract = split[1];
                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "contract");

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                //xoa anh pcontract
                folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PContractImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[2]);

                string[] imageFiles = Directory.GetFiles(folderItem);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);
                System.IO.File.Delete(pdfPath);
            }
            else
            {
                //Báo lõi
                TempData["SwalMessageType"] = "error";
                TempData["SwalMessageIcon"] = "error";
                TempData["SwalMessageTitle"] = "Ký không thành công !!";
                return BadRequest();
            }

            Customer customerDoing = await _customerService.GetCustomerById(HttpContext.Session.GetString(SessionKey.Customer.CustomerID));
            API.OperationHistoryCus historyCus = new API.OperationHistoryCus()
            {
                OperationName = $"{customerDoing.FullName} - ID:{customerDoing.CustomerId.ToString().Substring(0, 8)} đã ký hợp đồng - ID:{idDContract[1]}.",
                CustomerID = customerDoing.CustomerId
            };
            await _historyCusSvc.AddNew(historyCus);

            HttpContext.Session.SetString(SessionKey.Customer.CustomerID, "");
            HttpContext.Session.SetString(SessionKey.PedningContract.PContractID, "");
            HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, "");

            _uploadHelper.RemoveImage(filePath);
            //xem thông tin hiển thị alert ở CusToSign hàm SaveSign(), vì dùng http nên không có trả ở return nó sẽ chạy hết hàm SaveSign(). Các action khác mà được gọi từ ajax
            //cũng hiển thị thông báo tương tự
            return Ok();
        }

        /*[HttpPost("SignContractWithUsbToken")]
        public async Task<ActionResult> SignContractWithUsbToken([FromBody] DoneContract dContract)
        {
            if (dContract.Base64File != null)
            {
                var customerID = _pContractService.getByIdAsnyc(dContract.PContractID.ToString()).Result.CustomerId;
                var customerDoing = await _customerService.GetCustomerById(customerID);
                var result = await _dContractsService.SignContractWithUSBToken(dContract);

                if (result == null)
                {
                    return BadRequest();
                }
                string[] split = result.Split('*');
                string pdfPath = null;
                IFormFile file = _uploadHelper.ConvertBase64ToIFormFile(split[0], Guid.NewGuid().ToString().Substring(0, 8), "application/pdf");
                pdfPath = _uploadHelper.UploadPDF(file, _hostingEnvironment.WebRootPath, "TempFile", ".pdf");
                _pdfToImageHelper.PdfToPng(pdfPath, int.Parse(split[1]), "contract");

                //xoa anh pcontract
                var folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "PContractImage"); // + thêm ID của contract

                string folderItem = System.IO.Path.Combine(folderPath, split[2]);

                string[] imageFiles = Directory.GetFiles(folderItem);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);
                System.IO.File.Delete(pdfPath);


                API.OperationHistoryCus historyCus = new API.OperationHistoryCus()
                {
                    OperationName = $"{customerDoing.FullName} - ID:{customerDoing.CustomerId.ToString().Substring(0, 8)} đã ký hợp đồng bằng USBToken - ID:{split[1]}.",
                    CustomerID = customerDoing.CustomerId
                };
                await _historyCusSvc.AddNew(historyCus);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }*/

        //Trang cho khách hàng xem hđ và bb lắp
        public async Task<ActionResult> ShowDContract(string token)
        {
            HttpContext.Session.SetString(SessionKey.Customer.CustomerToken, token);
            int id = DecodeTokenDContract(token);
           var dContract = await _dContractsService.getByIdAsnyc(id.ToString());
            if(dContract != null)
            {
                return View(dContract);
            }
            else
            {
                //báo lỗi ko tìm thấy hợp dồng
                TempData["SwalMessageType"] = "error";
                TempData["SwalMessageIcon"] = "error";
                TempData["SwalMessageTitle"] = "Không tìm thấy hợp đồng!!";
                return BadRequest();
            }
        }


        public async Task<ActionResult> UnEffectDContract(string token)
        {
            HttpContext.Session.SetString(SessionKey.Customer.CustomerToken, token);
            int id = DecodeTokenDContract(token);
            var dContract = await _dContractsService.getByIdUnEffect(id.ToString());
            if (dContract != null)
            {
                dContract.IsInEffect = false;
                var result = await _dContractsService.updateIsEffect(dContract);

                if (result == null)
                {
                    return View("Error");
                }

                var folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "MinuteImage");

                string folderItem = System.IO.Path.Combine(folderPath, result.DoneMinuteId.ToString());

                string[] imageFiles = Directory.GetFiles(folderItem);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);


                folderPath = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, "ContractImage");

                folderItem = System.IO.Path.Combine(folderPath, result.DContractID);

                string[] imageContractFiles = Directory.GetFiles(folderItem);

                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                foreach (string imageFile in imageContractFiles)
                {
                    System.IO.File.Delete(imageFile);
                }
                Directory.Delete(folderItem);

                return View("Index");
            }
            else
            {
                //báo lỗi ko tìm thấy hợp dồng
                TempData["SwalMessageType"] = "error";
                TempData["SwalMessageIcon"] = "error";
                TempData["SwalMessageTitle"] = "Không tìm thấy hợp đồng!!";
                return BadRequest();
            }


        }

        private int DecodeTokenDContract(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            int dContractId = int.Parse(tokenS.Claims.First(claim => claim.Type == "DContractID").Value);
            return dContractId;
        }
    }
}
