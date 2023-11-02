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

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigningController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IPendingContractSvc _pendingContract;
        private readonly IContractCoordinateSvc _contractCoordinateSvc;
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

        public SigningController(IPFXCertificateSvc pfxCertificate, IInstallationRequirementSvc requirementSvc, IDoneContractSvc dContractSvc,
            IPendingContractSvc pendingContract, ITemplateContractSvc templateContractSvc, IEmployeeSvc employeeSvc, ICustomerSvc customerSvc,
            IGenerateQRCodeHelper generateQRCodeHelper, IConfiguration configuration, IPdfToImageHelper pdfToImageHelper, IUploadFileHelper uploadFileHelper,
            ISendMailHelper sendMailHelper, IContractCoordinateSvc contractCoordinateSvc)
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
        }

        //chưa test khi dùng chữ ký hết hạn
        [HttpPost("SignDirector")]
        public async Task<ActionResult<string>> ContractingDirector(string serial, int idContract, string imagePath)
        {
            var certi = await _pfxCertificate.GetById(serial);
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

            TemplateContract tContract = await _templateContractSvc.getByIdAsnyc(pContract.TContractId);
            DirectorZone directorZone = JsonConvert.DeserializeObject<DirectorZone>(tContract.jsonDirectorZone);

            var Coordinates = await _contractCoordinateSvc.getByTContract(pContract.TContractId);

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
                float y = 837 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
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
                float y = 835 - coordinate.Y; // Lấy tọa độ Y từ bảng toạ độ
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

            pdfStamper.Close();
            pdfReader.Close();

            FileStream fsPContract1 = new System.IO.FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fsPContract1.Close();

            var signedContractPath = await _pfxCertificate.SignContract(imagePath, pContract.PContractFile, pContract.PContractFile, certi.Serial, directorZone.X, directorZone.Y);

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
                TContractId = pContract.TContractId
            };
            var customer = await _customerSvc.GetByIdAsync(pContract.CustomerId.ToString());
            var url = GenerateUrl(pContract.PContractID);
            var qrPath = _generateQRCodeHelper.GenerateQRCode(url, pContract.PContractID);
            var sendMail = SendMailToCustomer(qrPath, url, customer);
            _pdfToImageHelper.PdfToPng(pContract.PContractFile, pendingContract.PContractId);
            await _pendingContract.updateAsnyc(pendingContract);
            return Ok(signedContractPath);
        }

        //chưa test khi dùng chữ ký hết hạn
        [HttpPost("SignCustomer")]
        public async Task<ActionResult<string>> ContractingCustomer(string serial, int idContract, string imagePath)
        {
            var certi = await _pfxCertificate.GetById(serial);
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

            TemplateContract tContract = await _templateContractSvc.getByIdAsnyc(pContract.TContractId);
            CustomerZone customerZone = JsonConvert.DeserializeObject<CustomerZone>(tContract.jsonCustomerZone);

            string outputContract = pContract.PContractFile.Replace("PContracts", "DContracts");

            if (!Directory.Exists($"AppData/DContracts/{pContract.PContractID}"))
            {
                Directory.CreateDirectory($"AppData/DContracts/{pContract.PContractID}");
            }

            var signedContractPath = await _pfxCertificate.SignContract(imagePath, pContract.PContractFile, outputContract, certi.Serial, customerZone.X, customerZone.Y);

            FileStream fs = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(pContract.PContractFile);
            string qrCodePath = pContract.PContractFile.Replace(".pdf", ".png");
            FileStream fs1 = new FileStream(qrCodePath, FileMode.Open, FileAccess.Read);
            fs1.Close();
            System.IO.File.Delete(qrCodePath);
            Directory.Delete($"AppData/PContracts/{pContract.PContractID}");

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
                TContractId = pContract.TContractId
            };

            _pdfToImageHelper.PdfToPng(outputContract, pendingContract.PContractId);
            var dContract = await _dContractSvc.addAsnyc(pendingContract);
            await _pendingContract.deleteAsnyc(pendingContract.PContractId);

            InstallationRequirement requirement = new InstallationRequirement()
            {
                DateCreated = DateTime.Now,
                MinuteName = "Biên bản lắp đặt hợp đồng" + dContract,
                DoneContractId = int.Parse(dContract),
                MinuteFile = "",
                TMinuteId = 1
            };
            var result = await _requirementSvc.CreateIRequirement(requirement);

            return Ok(signedContractPath);
        }

        //function test
        [HttpGet("GetByToken/{token}")]
        public async Task<ActionResult<string>> GetByToken(string token)
        {
            // Giải mã token để lấy id khách hàng và id hợp đồng
            var contractID = DecodeToken(token);

            // Lấy thông tin hợp đồng dựa trên customerId và contractId
            var contract = GetContract(contractID);

            if (contract == null)
            {
                return NotFound("Không tìm thấy hợp đồng.");
            }

            // Hiển thị hợp đồng cho khách hàng
            return Ok(await contract);
        }

        private string GenerateToken(int contractID)
        {
            List<Claim> claims = new List<Claim>() {
                 new Claim("ContractID", contractID.ToString())
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
            //Tạo token với id khách hàng và id hợp đồng
            var token = GenerateToken(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)
            var url = $"https://localhost:7286/api/Signing/GetByToken/{token}";

            // Gửi URL cho khách hàng
            return url;
        }

        private int DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var pcontractID = int.Parse(tokenS.Claims.First(claim => claim.Type == "ContractID").Value);
            return pcontractID;
        }

        private async Task<PendingContract> GetContract(int contractID)
        {
            return await _pendingContract.getByIdAsnyc(contractID);
        }

        private async Task<string> SendMailToCustomer(string qrPath, string url, Customer customer)
        {
            string content = $"<body>" +
                                 $"<div style=\"font-family: Arial, sans-serif; background-color: #f2f2f2; margin: 0; padding: 0;\"" +
                                    $"<div style=\"background-color: #fff; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ccc; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1;\"" +
                                        $"<h1 style=\"color: #653AFE;\">Chào {customer.FullName}, cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</h1>" +
                                            $"<p style=\"color: #333;\">Dưới đây là đường dẫn để xem và ký hợp đồng:</p>" +
                                                $"<div style=\"text-align: center;\">" +
                                                      $"<p><a style=\"display: inline-block; padding: 10px 20px; background-color: #33BDFE; color: #fff; text-decoration: none; border: none; border-radius: 5px;\" href=\"{url}\">Ký Hợp Đồng</a></p>" +
                                                $"</div>" +
                                                $"<div style=\"text-align: center;\">" +
                                                    $"<img style=\"max-width: 200px; height: auto;\" src=\"{qrPath}\" alt=\"QRCode\">" +
                                                $"</div>" +
                                      $"</div>" +
                                 $"</div>"+
                             $"</body>";

            SendMail mail = new SendMail();
            mail.Subject = "Hợp đồng Từ TechSeal";
            mail.ReceiverName = customer.FullName;
            mail.ToMail = customer.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMail(mail);
            if (isSuccess != null)
            {
                return "Đã cấp mật khẩu mới";
            }
            else
            {
                return null;
            }
        }
    }
}