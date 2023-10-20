using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PFXCertificatesController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IEncodeHelper _encodeHelper;
        private readonly IUploadFileHelper _uploadFileHelper;
        public PFXCertificatesController(IPFXCertificateSvc pfxCertificate, IEncodeHelper encodeHelper, IUploadFileHelper uploadFileHelper)
        {
            _pfxCertificate = pfxCertificate;
            _encodeHelper = encodeHelper;
            this._uploadFileHelper = uploadFileHelper;
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
        public async Task<ActionResult<string>> UpdateNotAfter(string pfxFilePath, string password, bool isEmployee)
        {
            string isUpdateToDatabase = null;

            PFXCertificate isUpdateNotAfter = await _pfxCertificate.UpdateNotAfter(pfxFilePath, _encodeHelper.Encode(password), isEmployee);

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

        [HttpPut("UploadImage/{serial}")]
        public async Task<ActionResult<string>> UploadSignatureImage(string serial, IFormFile file)
        {
            PFXCertificate certificateExist = await _pfxCertificate.GetById(serial);
            try
            {
                if (file != null)
                {
                    if (file.ContentType.StartsWith("image/"))
                    {
                        if (file.Length > 0)
                        {
                            if (certificateExist.ImageSignature1 == null)
                            {
                                certificateExist.ImageSignature1 = _uploadFileHelper.UploadFile(file, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature2 == null)
                            {
                                certificateExist.ImageSignature2 = _uploadFileHelper.UploadFile(file, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature3 == null)
                            {
                                certificateExist.ImageSignature3 = _uploadFileHelper.UploadFile(file, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature4 == null)
                            {
                                certificateExist.ImageSignature4 = _uploadFileHelper.UploadFile(file, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature5 == null)
                            {
                                certificateExist.ImageSignature5 = _uploadFileHelper.UploadFile(file, "AppData", $"SignatureImages/{certificateExist.Serial}");
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

        [HttpDelete("DeleteImage/{serial}")]
        public async Task<ActionResult<string>> DeleteImage(string serial, string filePath)
        {
            PFXCertificate certificateExist = await _pfxCertificate.GetById(serial);
            
            if (certificateExist.ImageSignature1 == filePath)
            {
                _uploadFileHelper.RemoveFile(filePath);
                certificateExist.ImageSignature1 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
               
            }
            if (certificateExist.ImageSignature2 == filePath)
            {
                _uploadFileHelper.RemoveFile(filePath);
                certificateExist.ImageSignature2 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature3 == filePath)
            {
                _uploadFileHelper.RemoveFile(filePath);
                certificateExist.ImageSignature3 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature4 == filePath)
            {
                _uploadFileHelper.RemoveFile(filePath);
                certificateExist.ImageSignature4 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }
            if (certificateExist.ImageSignature5 == filePath)
            {
                _uploadFileHelper.RemoveFile(filePath);
                certificateExist.ImageSignature5 = null;
                return Ok(await _pfxCertificate.UpdateInfoToDatabase(certificateExist));
            }

            return BadRequest("Không thể xoá ảnh, hãy kiểm tra lại !");
        }
    }
}
