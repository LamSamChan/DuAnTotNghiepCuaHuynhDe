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

        private int isAuthenticate;
        private string employeeId;

        public CustomerController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IPContractService pContractService,
            ICustomerService customerService, ISigningService signingService, IPFXCertificateServices pfxCertificateServices)
        {
            _hostingEnvironment = hostingEnvironment;
            _contextAccessor = contextAccessor;
            _pContractService = pContractService;
            _customerService = customerService;
            _signingService = signingService;
            _pContractService = pContractService;
            _pfxCertificateServices = pfxCertificateServices;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> CusToSign(string token)
        {
            int pContractId = DecodeToken(token);

            VMDetailsContractAwait vm = new VMDetailsContractAwait();
            try
            {
                vm.PContract = await _pContractService.getByIdAsnyc(pContractId.ToString());
                vm.Customer = await _customerService.GetCustomerById(vm.PContract.CustomerId);
                vm.PFXCertificate = await _pfxCertificateServices.GetById(vm.Customer.SerialPFX);

                HttpContext.Session.SetString(SessionKey.PedningContract.PContractID, pContractId.ToString());
                HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, vm.Customer.SerialPFX);
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
        public ActionResult SaveSignature([FromBody] SignData sData)
        {
            string pContractID = HttpContext.Session.GetString(SessionKey.PedningContract.PContractID);
            string serial = HttpContext.Session.GetString(SessionKey.PFXCertificate.Serial);
            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid().ToString().Substring(0,8) + ".png";
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

            var respone = _signingService.SignContractByCustomer(signing);
            HttpContext.Session.SetString(SessionKey.PedningContract.PContractID,"");
            HttpContext.Session.SetString(SessionKey.PFXCertificate.Serial, "");
            return Content(fileName);
        }
    }
}
