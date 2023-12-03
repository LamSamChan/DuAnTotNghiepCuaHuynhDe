using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Helpers;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.DContractsServices;
using QuanLyHopDongVaKySo.CLIENT.Services.HistoryServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PFXCertificateServices;
using QuanLyHopDongVaKySo.CLIENT.Services.SigningServices;
using VMAPI = QuanLyHopDongVaKySo_API.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Constants;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsbTokenController : ControllerBase
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

        public UsbTokenController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor contextAccessor, IPContractService pContractService,
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
        [HttpPost("SignContractWithUsbToken")]
        public async Task<ActionResult> SignContractWithUsbToken([FromBody] DoneContract dContract)
        {
            if (dContract.Base64File != null)
            {
                string[] splitBase64 = dContract.Base64File.Split('*');
                HttpContext.Session.SetString(SessionKey.Customer.CustomerToken, splitBase64[1]);

                var customerID = _pContractService.getByIdAsnyc(dContract.PContractID.ToString()).Result.CustomerId;
                var customerDoing = await _customerService.GetCustomerById(customerID);

                DoneContract dContract2 = new DoneContract()
                {
                    PContractID = dContract.PContractID,
                    DConTractName = dContract.DConTractName,
                    Base64File = splitBase64[0]
                };
                var result = await _dContractsService.SignContractWithUSBToken(dContract2);

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
        }
    }
}
