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
            if (null == sData)
                return NotFound();

            var bmpSign = SignUtility.GetSignatureBitmap(sData.Data, sData.Smooth, _contextAccessor, _hostingEnvironment);

            var fileName = System.Guid.NewGuid() + ".png";

            var filePath = Path.Combine(Path.Combine(_hostingEnvironment.WebRootPath, "TempSignature"), fileName);


            bmpSign.Save(filePath, ImageFormat.Png);

            return Content(fileName);
        }
    }
}
