using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using System.Diagnostics.Contracts;
using System.Security.AccessControl;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFXCertificatesController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IPendingContractSvc _pendingContract;
        private readonly IEncodeHelper _encodeHelper;
        private readonly IUploadFileHelper _uploadFileHelper;
        private readonly ITemplateContractSvc _templateContractSvc;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ICustomerSvc _customerSvc;
        private readonly IDoneContractSvc _dContractSvc;
        private readonly IInstallationRequirementSvc _requirementSvc;

        public PFXCertificatesController(IPFXCertificateSvc pfxCertificate, IEncodeHelper encodeHelper, IUploadFileHelper uploadFileHelper,IInstallationRequirementSvc requirementSvc,  
            IDoneContractSvc dContractSvc,IPendingContractSvc pendingContract, ITemplateContractSvc templateContractSvc, IEmployeeSvc employeeSvc, ICustomerSvc customerSvc)
        {
            _pfxCertificate = pfxCertificate;
            _encodeHelper = encodeHelper;
            _uploadFileHelper = uploadFileHelper;
            _pendingContract = pendingContract;
            _templateContractSvc = templateContractSvc;
            _employeeSvc = employeeSvc;
            _customerSvc = customerSvc;
            _dContractSvc = dContractSvc;
            _requirementSvc = requirementSvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAll()
        {
            return Ok(await _pfxCertificate.GetAll());
        }
        [HttpGet("AboutToExpire")]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAllAboutToExpire()
        {
            return Ok(await _pfxCertificate.GetAllAboutToExpire());
        }

        [HttpGet("Expire")]
        public async Task<ActionResult<IEnumerable<PFXCertificate>>> GetAllExpire()
        {
            return Ok(await _pfxCertificate.GetAllExpire());
        }

        [HttpGet("{serial}")]
        public async Task<ActionResult<PFXCertificate>> GetById(string serial)
        {
            PFXCertificate certificate = await _pfxCertificate.GetById(serial);
            if (certificate != null)
            {
                return Ok(certificate);
            }
            else
            {
                return BadRequest(certificate);
            }
        }

        [HttpPost("UpdateNotAfter")]
        public async Task<ActionResult<string>> UpdateNotAfter(string serial)
        {

            var certi = await _pfxCertificate.GetById(serial);

            string isUpdateToDatabase = null;

            PFXCertificate isUpdateNotAfter = await _pfxCertificate.UpdateNotAfter(certi.PfxFilePath, certi.PfxPassword, certi.IsEmployee);

            if (isUpdateNotAfter != null)
            {
                isUpdateToDatabase = await _pfxCertificate.UpdateInfoToDatabase(isUpdateNotAfter);

                if (isUpdateToDatabase != null)
                {
                    return Ok(isUpdateToDatabase);
                }
                else
                {
                    return BadRequest(isUpdateToDatabase);
                }

            }
            else { return BadRequest(isUpdateNotAfter); }

        }

        [HttpPut("UploadImage/{serial}/{base64StringImage}")]
        public async Task<ActionResult<string>> UploadSignatureImage(string serial, string base64StringImage)
        {
            PFXCertificate certificateExist = await _pfxCertificate.GetById(serial);
            string filename = Guid.NewGuid().ToString().Substring(0,8);
            IFormFile imageFile = _uploadFileHelper.ConvertBase64ToIFormFile(base64StringImage, filename, "image/jpeg");
            try
            {
                if (imageFile != null)
                {
                    if (imageFile.ContentType.StartsWith("image/"))
                    {
                        if (imageFile.Length > 0)
                        {
                            if (certificateExist.ImageSignature1 == null)
                            {
                                certificateExist.ImageSignature1 = _uploadFileHelper.UploadFile(imageFile, "../QuanLyHopDongVaKySo.CLIENT/wwwroot", $"SignatureImages/{certificateExist.Serial}",".jpeg").
                                    Replace("../QuanLyHopDongVaKySo.CLIENT/wwwroot/", "");
                            }
                            else if (certificateExist.ImageSignature2 == null)
                            {
                                certificateExist.ImageSignature2 = _uploadFileHelper.UploadFile(imageFile, "../QuanLyHopDongVaKySo.CLIENT/wwwroot", $"SignatureImages/{certificateExist.Serial}", ".jpeg").
                                    Replace("../QuanLyHopDongVaKySo.CLIENT/wwwroot/", ""); ;
                            }
                            else if (certificateExist.ImageSignature3 == null)
                            {
                                certificateExist.ImageSignature3 = _uploadFileHelper.UploadFile(imageFile, "../QuanLyHopDongVaKySo.CLIENT/wwwroot", $"SignatureImages/{certificateExist.Serial}", ".jpeg").
                                    Replace("../QuanLyHopDongVaKySo.CLIENT/wwwroot/", ""); ;
                            }
                            else if (certificateExist.ImageSignature4 == null)
                            {
                                certificateExist.ImageSignature4 = _uploadFileHelper.UploadFile(imageFile, "../QuanLyHopDongVaKySo.CLIENT/wwwroot", $"SignatureImages/{certificateExist.Serial}", ".jpeg").
                                    Replace("../QuanLyHopDongVaKySo.CLIENT/wwwroot/", ""); ;
                            }
                            else if (certificateExist.ImageSignature5 == null)
                            {
                                certificateExist.ImageSignature5 = _uploadFileHelper.UploadFile(imageFile, "../QuanLyHopDongVaKySo.CLIENT/wwwroot", $"SignatureImages/{certificateExist.Serial}", ".jpeg").
                                    Replace("../QuanLyHopDongVaKySo.CLIENT/wwwroot/", ""); ;
                            }
                            else
                            {
                                return BadRequest("Kho ảnh chữ ký đã đầy, Hãy xóa 1 ảnh chữ ký để có thể tải lên ảnh chữ ký mới!");
                            }
                        }
                    }
                    else
                    {
                        return BadRequest("Hãy tải lên tệp có định dạng là ảnh !");
                    }

                    return await _pfxCertificate.UpdateInfoToDatabase(certificateExist);
                }
                else
                {
                    return BadRequest("Hãy tải lên ảnh chữ ký của bạn");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("DeleteImage/{serial}/{filePath}")]
        public async Task<ActionResult<string>> DeleteImage(string serial, string filePath)
        {
            PFXCertificate certificateExist = await _pfxCertificate.GetById(serial);
            
            if (certificateExist.ImageSignature1 == filePath)
            {
                _uploadFileHelper.RemoveFile("../QuanLyHopDongVaKySo.CLIENT/wwwroot/" + filePath);
                certificateExist.ImageSignature1 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
               
            }
            if (certificateExist.ImageSignature2 == filePath)
            {
                _uploadFileHelper.RemoveFile("../QuanLyHopDongVaKySo.CLIENT/wwwroot/" + filePath);
                certificateExist.ImageSignature2 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature3 == filePath)
            {
                _uploadFileHelper.RemoveFile("../QuanLyHopDongVaKySo.CLIENT/wwwroot/" + filePath);
                certificateExist.ImageSignature3 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature4 == filePath)
            {
                _uploadFileHelper.RemoveFile("../QuanLyHopDongVaKySo.CLIENT/wwwroot/" + filePath);
                certificateExist.ImageSignature4 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature5 == filePath)
            {
                _uploadFileHelper.RemoveFile("../QuanLyHopDongVaKySo.CLIENT/wwwroot/" + filePath);
                certificateExist.ImageSignature5 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }

            return BadRequest("Không thể xoá ảnh, hãy kiểm tra lại !");
        }

    }
}
