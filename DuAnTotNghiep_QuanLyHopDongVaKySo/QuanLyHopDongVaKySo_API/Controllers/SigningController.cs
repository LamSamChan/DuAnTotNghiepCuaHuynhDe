using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ContractInfo;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using QuanLyHopDongVaKySo_API.Services.DoneMinuteService;
using QuanLyHopDongVaKySo_API.ViewModels;
using static QRCoder.PayloadGenerator.SwissQrCode;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigningController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IPendingContractSvc _pendingContract;
        private readonly IPendingMinuteSvc _pendingMinuteSvc;
        private readonly IContractCoordinateSvc _contractCoordinateSvc;
        private readonly IMinuteCoordinateSvc _minuteCoordinateSvc;
        private readonly ITemplateContractSvc _templateContractSvc;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ICustomerSvc _customerSvc;
        private readonly IDoneContractSvc _dContractSvc;
        private readonly IInstallationRequirementSvc _requirementSvc;
        private readonly IConfiguration _configuration;
        private readonly IGenerateQRCodeHelper _generateQRCodeHelper;
        private readonly IPdfToImageHelper _pdfToImageHelper;
        private readonly IUploadFileHelper _uploadFileHelper;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        private readonly ITemplateMinuteSvc _templateMinuteSvc;
        private readonly IDoneMinuteSvc _doneMinuteSvc;


        public SigningController(IPFXCertificateSvc pfxCertificate, IInstallationRequirementSvc requirementSvc, IDoneContractSvc dContractSvc,
            IPendingContractSvc pendingContract, ITemplateContractSvc templateContractSvc, IEmployeeSvc employeeSvc, ICustomerSvc customerSvc,
            IGenerateQRCodeHelper generateQRCodeHelper, IConfiguration configuration, IPdfToImageHelper pdfToImageHelper, IUploadFileHelper uploadFileHelper,
            ISendMailHelper sendMailHelper, IContractCoordinateSvc contractCoordinateSvc, ITypeOfServiceSvc typeOfServiceSvc, 
            IPendingMinuteSvc pendingMinuteSvc, ITemplateMinuteSvc templateMinuteSvc, IMinuteCoordinateSvc minuteCoordinateSvc, IDoneMinuteSvc doneMinuteSvc)
        {
            _pfxCertificate = pfxCertificate;
            _pendingContract = pendingContract;
            _templateContractSvc = templateContractSvc;
            _employeeSvc = employeeSvc;
            _customerSvc = customerSvc;
            _dContractSvc = dContractSvc;
            _requirementSvc = requirementSvc;
            _configuration = configuration;
            _generateQRCodeHelper = generateQRCodeHelper;
            _pdfToImageHelper = pdfToImageHelper;
            _uploadFileHelper = uploadFileHelper;
            _sendMailHelper = sendMailHelper;
            _contractCoordinateSvc = contractCoordinateSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _pendingMinuteSvc = pendingMinuteSvc;
            _templateMinuteSvc = templateMinuteSvc;
            _minuteCoordinateSvc = minuteCoordinateSvc;
            _doneMinuteSvc = doneMinuteSvc;
        }

        [HttpPost("DirectorSignContract")]
        public async Task<ActionResult<string>> SignContractByDirector([FromBody] SigningModel signing)
        {
            int idContract = int.Parse(signing.IdFile);
            string serial = signing.Serial;
            string imagePath = null;
            string imagePathStamp = null;
            if (signing.Base64StringFile != null && signing.Base64StringFileStamp != null)
            {
                IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFile, Guid.NewGuid().ToString().Substring(0, 8), "image/jpeg");
                imagePath = _uploadFileHelper.UploadFile(file, "AppData", "SignatureImages", ".jpeg");
                IFormFile fileStamp = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFileStamp, Guid.NewGuid().ToString().Substring(0, 8), "image/png");
                imagePathStamp = _uploadFileHelper.UploadFile(fileStamp, "AppData", "SignatureImages", ".png");
            
            }
            else
            {
                return BadRequest();
            }
            
            var certi = await _pfxCertificate.GetById(serial);
            var expireCerti = await _pfxCertificate.GetAllExpire();

            if (expireCerti != null)
            {
                foreach (var pfx in expireCerti)
                {
                    if (certi.Serial == pfx.Serial)
                    {
                        return BadRequest("Chứng chỉ đã hết hạn, vui lòng gia hạn !");
                    }
                }
            }
            
            if (certi == null)
            {
                return BadRequest("Không có chứng chỉ hợp lệ !");
            }
            Employee director = null;

            if (certi.IsEmployee)
            {
                director = await _employeeSvc.GetBySerialPFX(serial);
            }
            else
            {
                return BadRequest("Chữ ký không hợp lệ");
            }

            var pContract = await _pendingContract.getByIdAsnyc(idContract);

            if (pContract.IsRefuse)
            {
                return BadRequest("Hợp đồng này đã bị từ chối duyệt!");
            }

            if (pContract.IsDirector)
            {
                return BadRequest("Hợp đồng này đã được giám đốc ký !");
            }

            if (pContract.IsCustomer)
            {
                return BadRequest("Hợp đồng này đã được khách hàng ký !");
            }
            var tContractID = _typeOfServiceSvc.GetById(pContract.TOS_ID).Result.templateContractID;
            TemplateContract tContract = await _templateContractSvc.getByIdAsnyc(tContractID);
            SignatureZone directorZone = JsonConvert.DeserializeObject<SignatureZone>(tContract.jsonDirectorZone);

            var Coordinates = await _contractCoordinateSvc.getByTContract(tContractID);

            FileStream fsPContract = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fsPContract.Close();
            System.IO.File.Delete(pContract.PContractFile);

            PdfReader pdfReader = new PdfReader(tContract.TContractFile);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(pContract.PContractFile, FileMode.Create));
            // Tạo một font cho trường văn bản
            BaseFont bf1 = BaseFont.CreateFont(@"AppData/texgyretermes-regular.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            // Thiết lập font và kích thước cho trường văn bản
            Font font1 = new Font(bf1, 10);
            var contract = await _pendingContract.ExportContract(pContract, director);
            foreach (var coordinate in Coordinates)
            {
                string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                float x = coordinate.X + 22; // Lấy tọa độ X từ bảng toạ độ
                float y = 839 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                var mappingName = ContractInternet.ContractFieldName.FirstOrDefault(id => id.Key == fieldName).Value;
                if (mappingName == null)
                {
                    continue;
                }
                PropertyInfo property = typeof(ContractInternet).GetProperty(mappingName);
                if (property != null)
                {
                    object value = property.GetValue(contract);
                    if (value != null)
                    {
                        string contractValue = value.ToString();
                        ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                        Element.ALIGN_BASELINE, new Phrase(contractValue, font1), x, y, 0);
                    }
                }
            }

            BaseFont bf2 = BaseFont.CreateFont(@"AppData/texgyretermes-bold.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            // Thiết lập font và kích thước cho trường văn bản
            Font font2 = new Font(bf2, 15);


            foreach (var coordinate in Coordinates)
            {
                string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                float x = coordinate.X + 30; // Lấy tọa độ X từ bảng toạ độ
                float y = 834 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                var mappingName = ContractInternet.RepresentativeContract.FirstOrDefault(id => id.Key == fieldName).Value;
                if (mappingName == null)
                {
                    continue;
                }
                PropertyInfo property = typeof(ContractInternet).GetProperty(mappingName);
                if (property != null)
                {
                    object value = property.GetValue(contract);
                    if (value != null)
                    {
                        string contractValue = value.ToString().ToUpper();
                        ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                        Element.ALIGN_BASELINE, new Phrase(contractValue, font2), x, y, 0);
                    }
                }
            }

            foreach (var coordinate in Coordinates)
            {
                string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                float x = coordinate.X + 30; // Lấy tọa độ X từ bảng toạ độ
                float y = 834 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                var mappingName = ContractInternet.CustomerSignNameInfo.FirstOrDefault(id => id.Key == fieldName).Value;
                if (mappingName == null)
                {
                    continue;
                }
                PropertyInfo property = typeof(ContractInternet).GetProperty(mappingName);
                if (property != null)
                {
                    object value = property.GetValue(contract);
                    if (value != null)
                    {
                        string contractValue = value.ToString().ToUpper();
                        ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                        Element.ALIGN_BASELINE, new Phrase(contractValue, font2), x, y, 0);
                    }
                }
            }

            pdfStamper.Close();
            pdfReader.Close();

            FileStream fsPContract1 = new System.IO.FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fsPContract1.Close();

            var signedContractPath = await _pfxCertificate.SignContract(imagePath, imagePathStamp, pContract.PContractFile, pContract.PContractFile, certi.Serial, directorZone.X, directorZone.Y, "contract");

            byte[] fileBytes = System.IO.File.ReadAllBytes(pContract.PContractFile);
            string base64String = Convert.ToBase64String(fileBytes);

            var customer = await _customerSvc.GetByIdAsync(pContract.CustomerId.ToString());

            
            PutPendingContract pendingContract = new PutPendingContract
            {
                PContractId = pContract.PContractID,
                DateCreated = pContract.DateCreated,
                PContractName = pContract.PContractName,
                PContractFile = pContract.PContractFile,
                IsDirector = true,
                IsCustomer = false,
                IsRefuse = pContract.IsRefuse,
                Reason = pContract.Reason,
                EmployeeCreatedId = pContract.EmployeeCreatedId,
                DirectorSignedId = director.EmployeeId,
                InstallationAddress = pContract.InstallationAddress,
                CustomerId = pContract.CustomerId,
                TOS_ID = pContract.TOS_ID,
                Base64File = base64String,
            };

            var url = GenerateUrl(pContract.PContractID);
            var qrPath = _generateQRCodeHelper.GenerateQRCode(url, pContract.PContractID);
            var sendMail = SendMailToCustomerWithImage(qrPath, url, customer);

            //_pdfToImageHelper.PdfToPng(pContract.PContractFile, pendingContract.PContractId,"contract");

            await _pendingContract.updateAsnyc(pendingContract);
            _uploadFileHelper.RemoveFile(imagePath);
            _uploadFileHelper.RemoveFile(imagePathStamp);

            return Ok(base64String+"*"+pContract.PContractID);
        }

        [HttpPost("CustomerSignContract")]
        public async Task<ActionResult<string>> SignContractByCustomer([FromBody] SigningModel signing)
        {

            int idContract = int.Parse(signing.IdFile);
            string serial = signing.Serial;
            string imagePath = null;

            if (signing.Base64StringFile != null)
            {
                IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFile, Guid.NewGuid().ToString().Substring(0, 8), "image/jpeg");
                imagePath = _uploadFileHelper.UploadFile(file, "AppData", "SignatureImages", ".jpeg");
            }
            else
            {
                return BadRequest();
            }

            var certi = await _pfxCertificate.GetById(serial);

            var expireCerti = await _pfxCertificate.GetAllExpire();

            if (expireCerti != null)
            {
                foreach (var pfx in expireCerti)
                {
                    if (certi.Serial == pfx.Serial)
                    {
                        return BadRequest("Chứng chỉ đã hết hạn, vui lòng gia hạn !");
                    }
                }
            }

            Customer customer = null;

            if (certi.IsEmployee)
            {
                return BadRequest("Chữ ký không hợp lệ");
            }
            else
            {
                customer = await _customerSvc.GetBySerialPFXAsync(serial);
            }

            var pContract = await _pendingContract.getByIdAsnyc(idContract);
            if (pContract == null)
            {
                return BadRequest("Hợp đông không tồn tại");
            }
            if (serial != customer.SerialPFX)
            {
                return BadRequest("Chữ ký không đúng với khách hàng của hợp đồng này");
            }

            if (pContract.IsRefuse)
            {
                return BadRequest("Hợp đồng này đã bị từ chối duyệt!");
            }

            if (!pContract.IsDirector)
            {
                return BadRequest("Hợp đồng này chưa được giám đốc ký !");
            }

            if (pContract.IsCustomer)
            {
                return BadRequest("Hợp đồng này đã được khách hàng ký !");
            }
            var tContractID = _typeOfServiceSvc.GetById(pContract.TOS_ID).Result.templateContractID;
            TemplateContract tContract = await _templateContractSvc.getByIdAsnyc(tContractID);
            SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tContract.jsonCustomerZone);

            string outputContract = pContract.PContractFile.Replace("PContracts", "DContracts");

            if (!Directory.Exists($"AppData/DContracts/{pContract.PContractID}"))
            {
                Directory.CreateDirectory($"AppData/DContracts/{pContract.PContractID}");
            }

            var signedContractPath = await _pfxCertificate.SignContract(imagePath,null, pContract.PContractFile, outputContract, certi.Serial, customerZone.X+20, customerZone.Y+10,"contract");

            FileStream fs = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(pContract.PContractFile);
            string qrCodePath = pContract.PContractFile.Replace(".pdf", ".png");
            FileStream fs1 = new FileStream(qrCodePath, FileMode.Open, FileAccess.Read);
            fs1.Close();
            System.IO.File.Delete(qrCodePath);
            Directory.Delete($"AppData/PContracts/{pContract.PContractID}");

            byte[] fileBytes = System.IO.File.ReadAllBytes(outputContract);
            string base64String = Convert.ToBase64String(fileBytes);

            PutPendingContract pendingContract = new PutPendingContract
            {
                PContractId = pContract.PContractID,
                DateCreated = pContract.DateCreated,
                PContractName = pContract.PContractName,
                PContractFile = outputContract,
                IsDirector = pContract.IsDirector,
                IsCustomer = true,
                IsRefuse = pContract.IsRefuse,
                Reason = pContract.Reason,
                EmployeeCreatedId = pContract.EmployeeCreatedId,
                DirectorSignedId = pContract.DirectorSignedId,
                CustomerId = pContract.CustomerId,
                InstallationAddress = pContract.InstallationAddress,
                TOS_ID = pContract.TOS_ID,
                Base64File = base64String
            };

            //_pdfToImageHelper.PdfToPng(outputContract, pendingContract.PContractId,"contract");


            var dContract = await _dContractSvc.addAsnyc(pendingContract);

            if (dContract == null)
            {
                return BadRequest("Them hop dong that bai");
            };
            await _pendingContract.deleteAsnyc(pendingContract.PContractId);

            string serviceName = _typeOfServiceSvc.GetById(dContract.TOS_ID).Result.ServiceName;
            InstallationRequirement requirement = new InstallationRequirement()
            {
                DateCreated = DateTime.Now,
                MinuteName = "Biên bản lắp đặt hợp đồng " + serviceName,
                DoneContractId = dContract.DContractID,
            };
            int result = await _requirementSvc.CreateIRequirement(requirement);

            _uploadFileHelper.RemoveFile(imagePath);

          
            if (result != 0)
            {
                var url = GenerateUrlShowDContract(dContract.DContractID);
                var sendMail = SendMailToCustomer(customer,url);
                //lấy PContractId xoá imagecontract của pending và tạo image của DContractID
                return Ok(base64String +"*" + dContract.DContractID+"*"+ pendingContract.PContractId);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("InstallerSignMinute")]
        public async Task<ActionResult<string>> SignMinuteByInstaller([FromBody] SigningModel signing)
        {

            int idMinute = int.Parse(signing.IdFile);
            string serial = signing.Serial;
            string imagePath = null;
            string imagePathStamp = null;

            if (signing.Base64StringFile != null && signing.Base64StringFileStamp != null)
            {
                IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFile, Guid.NewGuid().ToString().Substring(0, 8), "image/jpeg");
                imagePath = _uploadFileHelper.UploadFile(file, "AppData", "SignatureImages", ".jpeg");
                IFormFile fileStamp = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFileStamp, Guid.NewGuid().ToString().Substring(0, 8), "image/png");
                imagePathStamp = _uploadFileHelper.UploadFile(fileStamp, "AppData", "SignatureImages", ".png");
            }
            else
            {
                return BadRequest();
            }

            var certi = await _pfxCertificate.GetById(serial);
            var expireCerti = await _pfxCertificate.GetAllExpire();

            if (expireCerti != null)
            {
                foreach (var pfx in expireCerti)
                {
                    if (certi.Serial == pfx.Serial)
                    {
                        return BadRequest("Chứng chỉ đã hết hạn, vui lòng gia hạn !");
                    }
                }
            }

            if (certi == null)
            {
                return BadRequest("Không có chứng chỉ hợp lệ !");
            }
            Employee installer = null;

            if (certi.IsEmployee)
            {
                installer = await _employeeSvc.GetBySerialPFX(serial);
            }
            else
            {
                return BadRequest("Chữ ký không hợp lệ");
            }

            var pMinute = await _pendingMinuteSvc.GetById(idMinute);

            if (pMinute.IsIntallation)
            {
                return BadRequest("Biên bản này đã được nhân viên lắp đặt ký !");
            }

            if (pMinute.IsCustomer)
            {
                return BadRequest("Biên bản này đã được khách hàng ký !");
            }
            var dContract = await _dContractSvc.getByIdAsnyc(pMinute.DoneContractId.ToString());
            var tMinuteID = _typeOfServiceSvc.GetById(dContract.TOS_ID).Result.templateMinuteID;
            TemplateMinute tMinute = await _templateMinuteSvc.getByIdAsnyc(tMinuteID);
            SignatureZone signatureZone = JsonConvert.DeserializeObject<SignatureZone>(tMinute.jsonIntallationZone);

            var Coordinates = await _minuteCoordinateSvc.getByTMinute(tMinuteID);

            FileStream fsPMinute = new FileStream(pMinute.MinuteFile, FileMode.Open, FileAccess.Read);
            fsPMinute.Close();
            System.IO.File.Delete(pMinute.MinuteFile);

            PdfReader pdfReader = new PdfReader(tMinute.TMinuteFile);
            PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(pMinute.MinuteFile, FileMode.Create));
            // Tạo một font cho trường văn bản
            BaseFont bf1 = BaseFont.CreateFont(@"AppData/texgyretermes-regular.otf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            // Thiết lập font và kích thước cho trường văn bản
            Font font1 = new Font(bf1, 10);
            var minute = await _pendingMinuteSvc.ExportMinute(pMinute, installer.EmployeeId.ToString());
            minute.InstallationDate = dContract.DateDone.ToString("dd/MM/yyyy");

            foreach (var coordinate in Coordinates)
            {
                string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                float x = coordinate.X + 22; // Lấy tọa độ X từ bảng toạ độ
                float y = 839 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                var mappingName = MinuteInfo.MinuteFieldName.FirstOrDefault(id => id.Key == fieldName).Value;
                if (mappingName == null)
                {
                    continue;
                }
                PropertyInfo property = typeof(MinuteInfo).GetProperty(mappingName);
                if (property != null)
                {
                    object value = property.GetValue(minute);
                    if (value != null)
                    {
                        string minuteValue = value.ToString();
                        ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                        Element.ALIGN_BASELINE, new Phrase(minuteValue, font1), x, y, 0);
                    }
                }
            }

            foreach (var coordinate in Coordinates)
            {
                string fieldName = coordinate.FieldName; // Tên trường từ bảng toạ độ
                float x = coordinate.X + 22; // Lấy tọa độ X từ bảng toạ độ
                float y = 839 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
                var mappingName = MinuteInfo.Installation.FirstOrDefault(id => id.Key == fieldName).Value;
                if (mappingName == null)
                {
                    continue;
                }
                PropertyInfo property = typeof(MinuteInfo).GetProperty(mappingName);
                if (property != null)
                {
                    object value = property.GetValue(minute);
                    if (value != null)
                    {
                        string minuteValue = value.ToString().ToUpper();
                        ColumnText.ShowTextAligned(pdfStamper.GetOverContent(coordinate.SignaturePage),
                        Element.ALIGN_BASELINE, new Phrase(minuteValue, font1), x, y, 0);
                    }
                }
            }

            pdfStamper.Close();
            pdfReader.Close();

            FileStream fsPContract1 = new System.IO.FileStream(pMinute.MinuteFile, FileMode.Open, FileAccess.Read);
            fsPContract1.Close();

            var signedMinutePath = await _pfxCertificate.SignContract(imagePath,imagePathStamp, pMinute.MinuteFile, pMinute.MinuteFile, certi.Serial, signatureZone.X + 50, signatureZone.Y - 700, "minute");

            byte[] fileBytes = System.IO.File.ReadAllBytes(pMinute.MinuteFile);
            string base64String = Convert.ToBase64String(fileBytes);

            PutPMinute pendingMinute = new PutPMinute
            {
                PendingMinuteId = pMinute.PendingMinuteId,
                DateCreated = pMinute.DateCreated,
                MinuteName = pMinute.MinuteName,
                IsIntallation = true,
                IsCustomer = false,
                EmployeeId = pMinute.EmployeeId,
                DoneContractId = pMinute.DoneContractId,
                MinuteFile = pMinute.MinuteFile,
                Base64File = base64String
            };

           //_pdfToImageHelper.PdfToPng(pMinute.MinuteFile, pMinute.PendingMinuteId,"minute");
            await _pendingMinuteSvc.updateAsnyc(pendingMinute);

            FileStream fsPContract2 = new System.IO.FileStream(pMinute.MinuteFile, FileMode.Open, FileAccess.Read);
            fsPContract2.Close();

            _uploadFileHelper.RemoveFile(imagePath);
            _uploadFileHelper.RemoveFile(imagePathStamp);

            return Ok(base64String+"*"+pendingMinute.PendingMinuteId);
        }

        [HttpPost("CustomerSignMinute")]
        public async Task<ActionResult<string>> SignMinuteByCustomer([FromBody] SigningModel signing)
        {

            int idMinute = int.Parse(signing.IdFile);
            string serial = signing.Serial;
            string imagePath = null;
            if (signing.Base64StringFile != null)
            {
                IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(signing.Base64StringFile, Guid.NewGuid().ToString().Substring(0, 8), "image/jpeg");
                imagePath = _uploadFileHelper.UploadFile(file, "AppData", "SignatureImages", ".jpeg");
            }
            else
            {
                return BadRequest();
            }

            var certi = await _pfxCertificate.GetById(serial);

            var expireCerti = await _pfxCertificate.GetAllExpire();

            if (expireCerti != null)
            {
                foreach (var pfx in expireCerti)
                {
                    if (certi.Serial == pfx.Serial)
                    {
                        return BadRequest("Chứng chỉ đã hết hạn, vui lòng gia hạn !");
                    }
                }
            }

            Customer customer = null;

            if (certi.IsEmployee)
            {
                return BadRequest("Chữ ký không hợp lệ");
            }
            else
            {
                customer = await _customerSvc.GetBySerialPFXAsync(serial);
            }

            var pMinute = await _pendingMinuteSvc.GetById(idMinute);

            if (serial != customer.SerialPFX)
            {
                return BadRequest("Chữ ký không đúng với khách hàng của hợp đồng này");
            }

            if (!pMinute.IsIntallation)
            {
                return BadRequest("Hợp đồng này chưa được nhân viên lắp đặt ký !");
            }

            if (pMinute.IsCustomer)
            {
                return BadRequest("Hợp đồng này đã được khách hàng ký !");
            }

            var dContract = await _dContractSvc.getByIdAsnyc(pMinute.DoneContractId.ToString());
            var tMinuteID = _typeOfServiceSvc.GetById(dContract.TOS_ID).Result.templateMinuteID;
            TemplateMinute tMinute = await _templateMinuteSvc.getByIdAsnyc(tMinuteID);
            SignatureZone customerZone = JsonConvert.DeserializeObject<SignatureZone>(tMinute.jsonCustomerZone);

            string outputMinute = pMinute.MinuteFile.Replace("AppData/PMinutes", $"AppData/DContracts/{dContract.DContractID}");

            if (!Directory.Exists($"AppData/DContracts/{dContract.DContractID}"))
            {
                Directory.CreateDirectory($"AppData/DContracts/{dContract.DContractID}");
            }

            var signedMinutePath = await _pfxCertificate.SignContract(imagePath, null,pMinute.MinuteFile, outputMinute, certi.Serial, customerZone.X + 50, customerZone.Y - 700,"minute");

            FileStream fs = new FileStream(pMinute.MinuteFile, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(pMinute.MinuteFile);

            byte[] fileBytes = System.IO.File.ReadAllBytes(outputMinute);
            string base64String = Convert.ToBase64String(fileBytes);

            DoneMinute doneMinute = new DoneMinute()
            {
                DateDone = DateTime.Now,
                MinuteName = pMinute.MinuteName,
                MinuteFile = outputMinute,
                EmployeeId = pMinute.EmployeeId,
                Base64File = base64String
            };


 //           _pdfToImageHelper.PdfToPng(outputMinute, pMinute.PendingMinuteId, "minute");
            var dMinute = await _doneMinuteSvc.AddNew(doneMinute);
            dContract.DoneMinuteId = dMinute;

            PutDContract putDContract = new PutDContract()
            {
                DContractID = dContract.DContractID.ToString(),
                DateDone = dContract.DateDone,
                DContractName = dContract.DConTractName,
                DContractFile = dContract.DContractFile,
                IsInEffect = dContract.IsInEffect,
                InstallationAddress = dContract.InstallationAddress,
                EmployeeCreatedId = dContract.EmployeeCreatedId,
                DirectorSignedId = dContract.DirectorSignedId,
                CustomerId = dContract.CustomerId,
                TOS_ID = dContract.TOS_ID,
                DoneMinuteId = dContract.DoneMinuteId,
            };

            var updatedContract = await _dContractSvc.updateAsnyc(putDContract);

            int resutl = await _pendingMinuteSvc.DeletePMinute(pMinute.PendingMinuteId);

            var sendMail = SendMailToCustomerWithFile(System.IO.File.ReadAllBytes(dContract.DContractFile), System.IO.File.ReadAllBytes(outputMinute),customer);

            FileStream fsMinute = new System.IO.FileStream(outputMinute, FileMode.Open, FileAccess.Read);
            fsMinute.Close();

            _uploadFileHelper.RemoveFile(imagePath);

            if (resutl != 0)
            {
                return Ok(base64String +"*"+ dMinute + "*"+pMinute);
            }
            else
            {
                return BadRequest();
            }

        }

        

        private string GenerateToken(int contractID)
        {
            List<Claim> claims = new List<Claim>() {
                 new Claim("ContractID", contractID.ToString()),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
              _configuration["AppSettings:Token"]!));

            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creads
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string GenerateUrl(int contractID)
        {
            //Tạo token với id khách hàng và id hợp đồng + serial pfx
            var token = GenerateToken(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)

            //url locallhost
            var url = $"https://localhost:7063/Customer/CusToSign?token={token}";

            //url servcer
            //var url = $"https://techseal.azurewebsites.net/Customer/CusToSign?token={token}";

            // Gửi URL cho khách hàng
            return url;
        }

        private string GenerateTokenShowDContract(int DContractID)
        {
            List<Claim> claims = new List<Claim>() {
                 new Claim("DContractID", DContractID.ToString()),
             };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
              _configuration["AppSettings:Token"]!));

            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(int.MaxValue),
                    signingCredentials: creads
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string GenerateUrlShowDContract(int contractID)
        {
            //Tạo token với id khách hàng và id hợp đồng + serial pfx
            var token = GenerateTokenShowDContract(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)

            //url locallhost
            var url = $"https://localhost:7063/Customer/ShowDContract?token={token}";

            //url servcer
            //var url = $"https://techseal.azurewebsites.net/Customer/ShowDContract?token={token}";

            // Gửi URL cho khách hàng
            return url;
        }

        private async Task<string> SendMailToCustomer(Customer customer, string url)
        {
            string content =

    $"<p>Xin chào <b>{customer.FullName}</b>,</p>" +
    $"<p>Chúc mừng bạn đã ký thành công hợp đồng!</p>" +
    $"<p>Dưới đây là đường dẫn để xem hợp đồng trực tuyến của bạn (ngoài ra sau khi hoàn tất lắp đặt bạn sẽ nhận được <b>hợp đồng</b> và <b>biên bản lắp đặt</b> PDF):</p>" +
    $"<div style=\"text-align: center;\">" +
         $"<p><a style=\"display: inline-block; padding: 10px 20px; background-color: #33BDFE; color: #fff; text-decoration: none; border: none; border-radius: 5px;\" href=\"{url}\">Xem hợp đồng trực tuyến</a></p>" +
    $"</div>" +                                                                                        
    $"<p>Vui lòng lưu trữ thông tin này một cách an toàn.</p>" +
    $"<p>Nếu bạn gặp bất kỳ vấn đề hoặc có câu hỏi, hãy liên hệ với chúng tôi tại <b>techseal.digitalsignature@gmail.com Hoặc Liên Hệ: 0339292975.</b></p>" +
    $"<p>Chúng tôi rất trân trọng và biết ơn vì bạn đã sử dụng <b>TechSeal - Contract Management & Digital Signature</b> và chúc bạn có một ngày tốt lành!</p> " +
    $"<p>Trân trọng,</p> " +
    $"<p>Tech Seal.</p>";

            SendMail mail = new SendMail();
            mail.Subject = "Chúc mừng bạn đã ký hợp đồng thành công";
            mail.ReceiverName = customer.FullName;
            mail.ToMail = customer.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMail(mail);
            if (isSuccess != null)
            {
                return "Đã gửi thành công";
            }
            else
            {
                return null;
            }
        }

        private async Task<string> SendMailToCustomerWithImage(byte[] qrPath, string url, Customer customer)
        {
            string imageBase64 = Convert.ToBase64String(qrPath);
            string content = $"<body>" +
                                 $"<div style=\"font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0;\"" +
                                    $"<div style=\"background-color: #fff; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ccc; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1;\"" +
                                        $"<h1 style=\"color: #653AFE;\">Chào {customer.FullName}, cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</h1>" +
                                            $"<p style=\"color: #333;\">Dưới đây là đường dẫn để xem và ký hợp đồng:</p>" +
                                                $"<div style=\"text-align: center;\">" +
                                                      $"<p><a style=\"display: inline-block; padding: 10px 20px; background-color: #33BDFE; color: #fff; text-decoration: none; border: none; border-radius: 5px;\" href=\"{url}\">Ký Hợp Đồng</a></p>" +
                                                $"</div>" +
                                      $"</div>" +
                                 $"</div>"+
                             $"</body>";

            SendMail mail = new SendMail();
            mail.Subject = "Hợp đồng Từ TechSeal";
            mail.ReceiverName = customer.FullName;
            mail.ToMail = customer.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMailWithImage(mail, qrPath);
            if (isSuccess != null)
            {
                return "Đã gửi thành công";
            }
            else
            {
                return null;
            }
        }

        private async Task<string> SendMailToCustomerWithFile(byte[] bytesContract, byte[] bytesMinute, Customer customer)
        {

            string content = $"<body>" +
                $"<div class=\"container\" style=\"max-width: 600px; margin: 0 auto; padding: 20px; text-align: center; background-color: #f7f7f7;\">" +
                $"<p style=\"font-size: 16px; line-height: 1.6; color: #333;\">Kính gửi khách hàng {customer.FullName},</p>" +
                $"<p style=\"font-size: 16px; line-height: 1.6; color: #333;\">Xin mời tải hợp đồng của bạn</p>" +
                $" <p style=\"font-size: 16px; line-height: 1.6; color: #333;\">Nếu bạn có bất kỳ câu hỏi hoặc cần thêm thông tin, xin vui lòng liên hệ với chúng tôi.</p>" +
                $" <p style=\"font-size: 16px; line-height: 1.6; color: #333;\">Xin cảm ơn!</p>" +
                $" <p style=\"font-size: 16px; line-height: 1.6; color: #333;\">TechSeal</p>" +
                $"</div>" +
                $"<body>";


            SendMail mail = new SendMail();
            mail.Subject = "Hợp đồng Từ TechSeal";
            mail.ReceiverName = customer.FullName;
            mail.ToMail = customer.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMailWithFile(mail, bytesContract, bytesMinute);
            if (isSuccess != null)
            {
                return "Đã gửi thành công";
            }
            else
            {
                return null;
            }
        }
    }
}