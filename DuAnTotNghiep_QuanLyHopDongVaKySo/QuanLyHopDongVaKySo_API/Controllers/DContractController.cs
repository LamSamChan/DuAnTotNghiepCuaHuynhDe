using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DContractController : ControllerBase
    {
        private readonly IDoneContractSvc _doneContractSvc;
        private readonly IConfiguration _configuration;
        private readonly ICustomerSvc _customerSvc;
        private readonly ISendMailHelper _sendMailHelper;
        public DContractController(IDoneContractSvc doneContractSvc, IConfiguration configuration, ICustomerSvc customerSvc,
            ISendMailHelper sendMailHelper)
        {
            _doneContractSvc = doneContractSvc;
            _configuration = configuration;
            _customerSvc = customerSvc;
            _sendMailHelper = sendMailHelper;
        }

        [HttpGet("getAllEffect")]
        public async Task<IActionResult> GetAllEffect()
        {
            return Ok(await _doneContractSvc.getListIsEffect());
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

        [HttpPut("Update")]
        public async Task<IActionResult> Update(PutDContract dContract)
        {
            return Ok (await _doneContractSvc.updateAsnyc(dContract));
        }

        [HttpPut("UpdateIsEffect")]
        public async Task<IActionResult> UpdateIsEffect([FromBody] PutDContract dContract)
        {
            return Ok(await _doneContractSvc.updateIsEffect(dContract));
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
                string url = GenerateUrl(dContractId);

                string content =

   $"<p>Xin chào <b>{customer.FullName}</b>,</p>" +
   $"<h3>Bạn hãy đọc kỹ trước khi nhấn nút</h3>" +
   $"<h2>Dưới đây là đường dẫn sẽ dừng hợp đồng khi bạn nhấn vào:</h2>" +
   $"<div style=\"text-align: center;\">" +
          $"<ul> " +
            $"<li>Mã hợp đồng: {respone.DContractID}</li>" +
            $"<li>Tên hợp đồng: {respone.DConTractName}</li>" +
            $"<li>Ngày hiệu lực: {respone.DateDone}</li>" +
            $"<li>Ngày kết thúc: {DateTime.Now.ToString("dd/MM/yyyy")}</li>" +
         $"</ul> " +
        $"<p><a style=\"display: inline-block; padding: 10px 20px; background-color: red; color: #fff; text-decoration: none; border: none; border-radius: 5px;\" href=\"{url}\">Xác nhận dừng hợp đồng</a></p>" +
   $"</div>" +
   $"<p>Vui lòng lưu trữ thông tin này một cách an toàn.</p>" +
   $"<p>Nếu bạn gặp bất kỳ vấn đề hoặc có câu hỏi, hãy liên hệ với chúng tôi tại <b>techseal.digitalsignature@gmail.com Hoặc Liên Hệ: 0339292975.</b></p>" +
   $"<p>Chúng tôi rất trân trọng và biết ơn vì bạn đã sử dụng <b>TechSeal - Contract Management & Digital Signature</b> và chúc bạn có một ngày tốt lành!</p> " +
   $"<p>Trân trọng,</p> " +
   $"<p>Tech Seal.</p>";

              
                SendMail mail = new SendMail();
                string subject = "Xác nhận kết thúc hợp đồng từ yêu cầu của bạn";
                byte[] bytes1 = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(subject));
                mail.Subject = Encoding.UTF8.GetString(bytes1);
                mail.Subject = "Xác nhận kết thúc hợp đồng";
                mail.ReceiverName = customer.FullName;
                mail.ToMail = customer.Email;
                mail.HtmlContent = content;
                string isSuccess = await _sendMailHelper.SendMail(mail);
                if (isSuccess != null)
                {
                    return isSuccess;
                }
                else
                {
                    return null;
                }
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

        private string GenerateUrl(int contractID)
        {
            //Tạo token với id khách hàng và id hợp đồng + serial pfx
            var token = GenerateToken(contractID);

            // Tạo đường link có chứa token
            // Đường dẫn đến nơi hiển thị hợp đồng (Client)

            //url locallhost
            var url = $"https://localhost:7063/Customer/UnEffectDContract?token={token}";

            //url servcer
            //var url = $"https://techseal.azurewebsites.net/Customer/UnEffectDContract?token={token}";

            // Gửi URL cho khách hàng
            return url;
        }

    }
}
