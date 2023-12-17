using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingContractService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DContractController : ControllerBase
    {
        private readonly IDoneContractSvc _doneContractSvc;
        private readonly IPendingContractSvc _pendingContractSvc;
        private readonly IConfiguration _configuration;
        private readonly ICustomerSvc _customerSvc;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IUploadFileHelper _uploadFileHelper;
        private readonly IInstallationRequirementSvc _requirementSvc;
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        private readonly IShortLinkHelper _shortLinkHelper;


        public DContractController(IDoneContractSvc doneContractSvc, IConfiguration configuration, ICustomerSvc customerSvc,
            ISendMailHelper sendMailHelper, IPendingContractSvc pendingContractSvc, IUploadFileHelper uploadFileHelper,
            IInstallationRequirementSvc requirementSvc, ITypeOfServiceSvc typeOfServiceSvc, IShortLinkHelper shortLinkHelper)
        {
            _doneContractSvc = doneContractSvc;
            _configuration = configuration;
            _customerSvc = customerSvc;
            _sendMailHelper = sendMailHelper;
            _pendingContractSvc = pendingContractSvc;
            _uploadFileHelper = uploadFileHelper;
            _requirementSvc = requirementSvc;
            _typeOfServiceSvc = typeOfServiceSvc;
            _shortLinkHelper = shortLinkHelper;

        }

        [HttpGet("getAllEffect")]
        public async Task<IActionResult> GetAllEffect()
        {
            return Ok(await _doneContractSvc.getListIsEffect());
        }

        [HttpGet("getAllNotInstallYet")]
        public async Task<IActionResult> GetAllNotInstallYet()
        {
            return Ok(await _doneContractSvc.getNotInstallYet());
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _doneContractSvc.GetAll());
        }

        [HttpGet("getAllView")]
        public async Task<IActionResult> GetAllView()
        {
            return Ok(await _doneContractSvc.getAllAsnyc());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getByIdForWinForm(string id)
        {
            return Ok(await _doneContractSvc.getByIdAsnyc(id));
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> getById(int id)
        {
            return Ok(await _doneContractSvc.getByIView(id));
        }

        [HttpGet("getByCustomerId/{id}")]
        public async Task<IActionResult> GetbyCusId(string id)
        {
            return Ok(await _doneContractSvc.getListByCusId(id));
        }
        [HttpGet("getByEmpId/{id}")]
        public async Task<IActionResult> GetbyEmpId(string id)
        {
            return Ok(await _doneContractSvc.getListByEmpId(id));
        }
        [HttpGet("getByDirectorId/{id}")]
        public async Task<IActionResult> GetbyDirectorId(string id)
        {
            return Ok(await _doneContractSvc.getListByDirectorId(id));
        }

        [HttpGet("getByIdUnEffect/{id}")]
        public async Task<IActionResult> getByIdUnEffect(int id)
        {
            var respone = await _doneContractSvc.getByIdAsnyc(id.ToString());
            PutDContract putDContract = new PutDContract()
            {
                DContractID = respone.DContractID.ToString(),
                DateDone = respone.DateDone,
                DContractName = respone.DConTractName,
                DContractFile = respone.DContractFile,
                IsInEffect = respone.IsInEffect,
                InstallationAddress = respone.InstallationAddress,
                EmployeeCreatedId = respone.EmployeeCreatedId,
                DirectorSignedId = respone.DirectorSignedId,
                CustomerId = respone.CustomerId,
                TOS_ID = respone.TOS_ID,
                DoneMinuteId = respone.DoneMinuteId,
                Base64File = respone.Base64File
            };
            return Ok(putDContract);
        }

        [HttpPost("SignContractWithUSBToken")]
        public async Task<IActionResult> SignByUSBToken([FromBody] PostDContract postDContract)
        {
            if (postDContract.Base64File == null)
            {
                return BadRequest();
            }
            var pContract = await _pendingContractSvc.getByIdAsnyc(postDContract.PContractID);
            if (pContract == null)
            {
                return BadRequest();
            }
            else
            {

                DoneContract dContract = new DoneContract()
                {
                    DateDone = DateTime.Now,
                    DConTractName = pContract.PContractName,
                    IsInEffect = true,
                    DContractFile = "",
                    InstallationAddress = pContract.InstallationAddress,
                    EmployeeCreatedId = pContract.EmployeeCreatedId,
                    DirectorSignedId = pContract.DirectorSignedId,
                    CustomerId = pContract.CustomerId,
                    TOS_ID= pContract.TOS_ID,
                };
                var result = await _doneContractSvc.AddDContractFromSignByUSBToken(dContract);
                string contractPath = null;
                
                if (result != null)
                {
                    IFormFile file = _uploadFileHelper.ConvertBase64ToIFormFile(postDContract.Base64File, dContract.DContractID.ToString()+"_SignedByUsbToken", "application/pdf");
                    contractPath = _uploadFileHelper.UploadFile(file, "AppData/DContracts", result.DContractID.ToString(), ".pdf");
                    result.Base64File = postDContract.Base64File;
                    result.DContractFile = contractPath;

                    var updateResult = await _doneContractSvc.UpdateContractFromSignByUSBToken(result);
                    if (updateResult != null)
                    {
                        string serviceName = _typeOfServiceSvc.GetById(dContract.TOS_ID).Result.ServiceName;
                           InstallationRequirement requirement = new InstallationRequirement()
                        {
                            DateCreated = DateTime.Now,
                            MinuteName = "Biên bản lắp đặt hợp đồng " + serviceName,
                            DoneContractId = dContract.DContractID,
                            InstallationAddress = dContract.InstallationAddress
                        };

                        int resultRequirement = await _requirementSvc.CreateIRequirement(requirement);

                        if (resultRequirement == 0)
                        {
                            return BadRequest();
                        }

                        string qrCodePath = pContract.PContractFile.Replace("_director_signed.pdf", ".png");
                        FileStream fs = new FileStream(pContract.PContractFile, FileMode.Open, FileAccess.Read);
                        fs.Close();
                        FileStream fs1 = new FileStream(qrCodePath, FileMode.Open, FileAccess.Read);
                        fs1.Close();
                        System.GC.Collect();
                        System.GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(pContract.PContractFile);                   
                        System.IO.File.Delete(qrCodePath);
                        Directory.Delete($"AppData/PContracts/{pContract.PContractID}");

                        await _pendingContractSvc.deleteAsnyc(pContract.PContractID);

                        var customer = await _customerSvc.GetByIdAsync(pContract.CustomerId.ToString());
                        var url = await GenerateUrlShowDContract(dContract.DContractID);

                        Task.Run(() => SendMailToCustomer(customer, url));

                        return Ok(updateResult.DContractID + "*" + pContract.PContractID);
                    }
                     return BadRequest();            
                }
                return BadRequest();
            }
            
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PutDContract dContract)
        {
            return Ok(await _doneContractSvc.updateAsnyc(dContract));
        }

        [HttpPut("UpdateIsEffect")]
        public async Task<IActionResult> UpdateIsEffect([FromBody] PutDContract dContract)
        {
            var result = await _doneContractSvc.updateIsEffect(dContract);

            if (result == null)
            {
                return BadRequest();
            }
            //xoa anh pminute
            var folderPath = System.IO.Path.Combine("AppData", "DContracts");

            string folderItem = System.IO.Path.Combine(folderPath, dContract.DContractID);

            string[] imageFiles = Directory.GetFiles(folderItem);

            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            foreach (string imageFile in imageFiles)
            {
                System.IO.File.Delete(imageFile);
            }
            Directory.Delete(folderItem);

            return Ok(result);
        }

        [HttpGet("UnEffectContract/{dContractId}")]
        public async Task<string> SendMailToCusComfirm(int dContractId)
        {
            var respone = await _doneContractSvc.getByIdAsnyc(dContractId.ToString());
            var customer = await _customerSvc.GetByIdAsync(respone.CustomerId.ToString());
            if (respone == null)
            {
                return null;
            }
            else
            {
                string url = await GenerateUrl(dContractId);

                string content = System.IO.File.ReadAllText("AppData\\TemplateSendMail\\xacnhandunghd.html").Replace("[TENKHACHHANG]", customer.FullName).Replace("[MAHOPDONG]", respone.DContractID.ToString())
                    .Replace("[TENHOPDONG]", respone.DConTractName).Replace("[NGAYHIEULUC]", respone.DateDone.ToString()).Replace("[NGAYKETTHUC]", DateTime.Now.ToString("dd/MM/yyyy")).Replace("[URL]",url);


                SendMail mail = new SendMail();
                mail.Subject = "XÁC NHẬN DỪNG HỢP ĐỒNG";
                mail.ReceiverName = customer.FullName;
                mail.ToMail = customer.Email;
                mail.HtmlContent = content;
                Task.Run(() => _sendMailHelper.SendMail(mail));

                return "Success";
            }
        }

        private string GenerateToken(int contractID)
        {
            List<Claim> claims = new List<Claim>() {
                 new Claim("DContractID", contractID.ToString()),
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

        private async Task<string> GenerateUrl(int contractID)
        {
            //Tạo token với id hợp đồng
            var token = GenerateToken(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)

            //url locallhost
           // var url = $"https://localhost:7063/Customer/UnEffectDContract?token={token}";

            //url servcer
            var url = $"https://techseal.azurewebsites.net/Customer/UnEffectDContract?token={token}";

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
                    expires: DateTime.Now.AddYears(10),
                    signingCredentials: creads
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task<string> GenerateUrlShowDContract(int contractID)
        {
            //Tạo token với id khách hàng và id hợp đồng + serial pfx
            var token = GenerateTokenShowDContract(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)

            //url locallhost
             //var url = $"https://localhost:7063/Customer/ShowDContract?token={token}";

            //url servcer
           var url = $"https://techseal.azurewebsites.net/Customer/ShowDContract?token={token}";
            string urlShort = await _shortLinkHelper.GenerateShortUrl(url);

            // Gửi URL cho khách hàng
            return urlShort;
        }

        private async Task<string> SendMailToCustomer(Customer customer, string url)
        {
            string content = System.IO.File.ReadAllText("AppData\\TemplateSendMail\\xemhd.html").Replace("[TENKHACHHANG]", customer.FullName).Replace("[URL]", url);
            


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

    }
}
