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
        public async Task<IActionResult> CusToSign(string pContractId, string customerId)
        {

            VMDetailsContractAwait vm = new VMDetailsContractAwait();
            try
            {
                vm.PContract = await _pContractService.getByIdAsnyc(pContractId);
                vm.Customer = await _customerService.GetCustomerById(customerId);
                vm.PFXCertificate = await _pfxCertificateServices.GetById(vm.Customer.SerialPFX);
            }
            catch
            {
                return RedirectToAction("Index");
            }
            return View(vm);
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
