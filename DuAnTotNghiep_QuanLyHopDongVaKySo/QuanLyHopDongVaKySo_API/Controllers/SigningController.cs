using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;
using QuanLyHopDongVaKySo_API.Services.TemplateContractService;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigningController : ControllerBase
    {
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IPendingContractSvc _pendingContract;
        private readonly ITemplateContractSvc _templateContractSvc;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ICustomerSvc _customerSvc;
        private readonly IDoneContractSvc _dContractSvc;
        private readonly IInstallationRequirementSvc _requirementSvc;
        private readonly IConfiguration _configuration;

        public SigningController(IPFXCertificateSvc pfxCertificate, IInstallationRequirementSvc requirementSvc, IDoneContractSvc dContractSvc,
            IPendingContractSvc pendingContract, ITemplateContractSvc templateContractSvc, IEmployeeSvc employeeSvc, ICustomerSvc customerSvc, IConfiguration configuration)
        {
            _pfxCertificate = pfxCertificate;
            _pendingContract = pendingContract;
            _templateContractSvc = templateContractSvc;
            _employeeSvc = employeeSvc;
            _customerSvc = customerSvc;
            _dContractSvc = dContractSvc;
            _requirementSvc = requirementSvc;
            _configuration = configuration;
        }

        //chưa test khi dùng chữ ký hết hạn
        [HttpPost("SignDirector")]
        public async Task<ActionResult<string>> ContractingDirector(string serial, int idContract, string imagePath)
        {
            var certi = await _pfxCertificate.GetById(serial);
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
                CustomerId = pContract.CustomerId,
                TOS_ID = pContract.TOS_ID,
                TContractId = pContract.TContractId
            };
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

            if (!Directory.Exists("AppData/DContracts/"))
            {
                Directory.CreateDirectory("AppData/DContracts/");
            }

            var signedContractPath = await _pfxCertificate.SignContract(imagePath, pContract.PContractFile, outputContract, certi.Serial, customerZone.X, customerZone.Y);

            FileStream fs = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
            fs.Close();
            System.IO.File.Delete(pContract.PContractFile);

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
                TOS_ID = pContract.TOS_ID,
                TContractId = pContract.TContractId
            };

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
            await _requirementSvc.CreateIRequirement(requirement);

            return Ok(signedContractPath);
        }

        // private string GenerateToken(int contractID, string customerID)
        // {
        //     List<Claim> claims = new List<Claim>() {
        //         new Claim("ContractID", contractID.ToString()),
        //         new Claim("CustomerID", customerID),
        //     };

        //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        //         _configuration["AppSettings:Token"]!));

        //     var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //     var token = new JwtSecurityToken(
        //             claims: claims,
        //             expires: DateTime.Now.AddDays(1),
        //             signingCredentials: creads
        //         );

        //     var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //     return jwt;
        // }

        // public string GenerateUrl(string customerId, int contractId)
        // {
        //     // Tạo token với id khách hàng và id hợp đồng
        //     var token = GenerateToken(contractId, customerId);

        //     // Tạo đường link có chứa token
        //     var url = $"/api/contracts/viewcontract?token={token}";

        //     // Gửi URL cho khách hàng
        //     return url;
        // }
    }
}