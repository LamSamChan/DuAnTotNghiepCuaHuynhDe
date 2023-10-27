using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
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



        public PFXCertificatesController(IPFXCertificateSvc pfxCertificate, IEncodeHelper encodeHelper, IUploadFileHelper uploadFileHelper, 
            IPendingContractSvc pendingContract, ITemplateContractSvc templateContractSvc, IEmployeeSvc employeeSvc, ICustomerSvc customerSvc)
        {
            _pfxCertificate = pfxCertificate;
            _encodeHelper = encodeHelper;
            _uploadFileHelper = uploadFileHelper;
            _pendingContract = pendingContract;
            _templateContractSvc = templateContractSvc;
            _employeeSvc = employeeSvc;
            _customerSvc = customerSvc;
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

        [HttpPut("UploadImage")]
        public async Task<ActionResult<string>> UploadSignatureImage(string serial, IFormFile imageFile)
        {
            PFXCertificate certificateExist = await _pfxCertificate.GetById(serial);
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
                                certificateExist.ImageSignature1 = _uploadFileHelper.UploadFile(imageFile, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature2 == null)
                            {
                                certificateExist.ImageSignature2 = _uploadFileHelper.UploadFile(imageFile, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature3 == null)
                            {
                                certificateExist.ImageSignature3 = _uploadFileHelper.UploadFile(imageFile, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature4 == null)
                            {
                                certificateExist.ImageSignature4 = _uploadFileHelper.UploadFile(imageFile, "AppData", $"SignatureImages/{certificateExist.Serial}");
                            }
                            else if (certificateExist.ImageSignature5 == null)
                            {
                                certificateExist.ImageSignature5 = _uploadFileHelper.UploadFile(imageFile, "AppData", $"SignatureImages/{certificateExist.Serial}");
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

        [HttpDelete("DeleteImage")]
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

        //chưa test khi dùng chữ ký hết hạn
        [HttpPost("SignContract")]
        public async Task<ActionResult<string>> SignContract(string serial,int idContract, string imagePath)
        {
            var certi = await _pfxCertificate.GetById(serial);
            Employee director = null;
            Customer customer = null;

            if (certi.IsEmployee)
            {
                director = await _employeeSvc.GetBySerialPFX(serial);
            }
            else
            {
                customer = await _customerSvc.GetBySerialPFXAsync(serial);
            }

            var pContract = await _pendingContract.getPContractAsnyc(idContract);

            if (customer != null)
            {
                if (serial != customer.SerialPFX)
                {
                    return BadRequest("Chữ ký không đúng với khách hàng của hợp đồng này");
                }
            }

            if (pContract.IsRefuse)
            {
                return BadRequest("Hợp đồng này đã bị từ chối duyệt!");
            }

            if (certi.IsEmployee && pContract.IsDirector)   
            {
                return BadRequest("Hợp đồng này đã được giám đốc ký !");
            }

            if (!certi.IsEmployee && !pContract.IsDirector)
            {
                return BadRequest("Hợp đồng này chưa được giám đốc ký !");
            }

            if (!certi.IsEmployee && pContract.IsCustomer)
            {
                return BadRequest("Hợp đồng này đã được khách hàng ký !");
            }

            TemplateContract tContract = await _templateContractSvc.getTContractAsnyc(pContract.TContractId);
            DirectorZone directorZone = JsonConvert.DeserializeObject<DirectorZone>(tContract.jsonCustomerZone);
            CustomerZone customerZone = JsonConvert.DeserializeObject<CustomerZone>(tContract.jsonCustomerZone);

            bool isDirector = false;
            bool isCustomer = false;

            if (certi.IsEmployee)
            {
                isDirector = true;
            }
            else
            {
                isDirector = true;
            }

            if (pContract.IsDirector)
            {
                isCustomer = true;
            }
            
            string outputContract = null;

            if (pContract.IsDirector && !certi.IsEmployee)
            {
                outputContract = pContract.PContractFile.Replace("PContracts", "DContracts");

                if (!Directory.Exists("AppData/DContracts/"))
                {
                    Directory.CreateDirectory("AppData/DContracts/");
                }
            }
            else
            {
                outputContract = pContract.PContractFile;
            }

            var signedContractPath = await _pfxCertificate.SignContract(imagePath, pContract.PContractFile, outputContract, certi.Serial, certi.IsEmployee ? directorZone.X : customerZone.X, certi.IsEmployee ? directorZone.Y : customerZone.Y);

            if (pContract.IsDirector && !certi.IsEmployee)
            {
                FileStream fs = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
                fs.Close();
                System.IO.File.Delete(pContract.PContractFile);
            }

            PutPendingContract pendingContract = new PutPendingContract();
            if (certi.IsEmployee)
            {
                pendingContract = new PutPendingContract
                {
                    PContractId = pContract.PContractID,
                    DateCreated = pContract.DateCreated,
                    PContractName = pContract.PContractName,
                    PContractFile = outputContract,
                    IsDirector = isDirector,
                    IsCustomer = isCustomer,
                    IsRefuse = pContract.IsRefuse,
                    Reason = pContract.Reason,
                    EmployeeCreatedId = pContract.EmployeeCreatedId,
                    DirectorSignedId = director.EmployeeId,
                    CustomerId = pContract.CustomerId,
                    TOS_ID = pContract.TOS_ID,
                    TContractId = pContract.TContractId
                };
            }
            else
            {
                pendingContract = new PutPendingContract
                {
                    PContractId = pContract.PContractID,
                    DateCreated = pContract.DateCreated,
                    PContractName = pContract.PContractName,
                    PContractFile = outputContract,
                    IsDirector = isDirector,
                    IsCustomer = isCustomer,
                    IsRefuse = pContract.IsRefuse,
                    Reason = pContract.Reason,
                    EmployeeCreatedId = pContract.EmployeeCreatedId,
                    DirectorSignedId = pContract.DirectorSignedId,
                    CustomerId = pContract.CustomerId,
                    TOS_ID = pContract.TOS_ID,
                    TContractId = pContract.TContractId
                };
            }

            await _pendingContract.updatePContractAsnyc(pendingContract);
            return Ok(signedContractPath);
        }

    }
}
