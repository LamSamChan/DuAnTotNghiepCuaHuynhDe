using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private static string otpChange;
        private static string otpForgot;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly IOTPGeneratorHelper _otpGeneratorHelper;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IRandomPasswordHelper _randomPasswordHelper;

        public PasswordController(IEmployeeSvc employeeSvc, IOTPGeneratorHelper otpGeneratorHelper, ISendMailHelper sendMailHelper, IRandomPasswordHelper randomPasswordHelper) { 
            _employeeSvc = employeeSvc;
            _otpGeneratorHelper = otpGeneratorHelper;
            _sendMailHelper = sendMailHelper;
            _randomPasswordHelper = randomPasswordHelper;
        }

        [HttpPost]
        public async Task<ActionResult<string>> ChangePassword([FromForm] ChangePassword changePassword, string employeeId, string comfirmOTP)
        {
            if (otpChange == comfirmOTP)
            {
                int? result = await _employeeSvc.ChangePassword(employeeId, changePassword);
                if (result != null)
                {
                    if (result == 0)
                    {
                        return BadRequest("Mật khẩu hiện tại không đúng !");
                    }
                    else
                    {
                        return Ok("Thay đổi mật khẩu thành công.");
                    }
                }
                else
                {
                    return BadRequest("Nhân viên không tồn tại !");
                }
            }
            else
            {
                return BadRequest("Mã xác nhận OTP không đúng!");
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<string>> ForgotPassword([FromForm] ForgotPassword forgotPassword, string comfirmOTP)
        {
            if (otpForgot == comfirmOTP)
            {
                string newPassword = await _randomPasswordHelper.GeneratePassword(8);
                int result = await _employeeSvc.ForgotPassword(newPassword, forgotPassword);
                if (result == 1)
                {

                    return Ok("Đã cấp mật khẩu mới");
                }
                else
                {
                    return BadRequest("Cấp mật khẩu mới không thành công");
                }
                
            }
            else
            {
                return BadRequest("Mã xác nhận OTP không đúng!");
            }

        }

        [HttpPost("GetOTPChange")]
        public async Task<ActionResult<string>> GetOTPChange(string employeeId)
        {

            var employee = await _employeeSvc.GetById(employeeId);
            string otp = await _otpGeneratorHelper.GenerateOTP(6);

            otpChange = otp;

            string content = $"<div style = \"font-family: Arial, sans-serif; padding: 20px; \">" +
        $"<h2> Xác thực bằng Mã OTP từ Tech Seal </h2>" + 
        $"<p> Cảm ơn bạn đã sử dụng dịch vụ của Tech Seal.</p>" +
        $"<p> Dưới đây là mã OTP của bạn:</p>" +
        $"<p style=\"font-size: 24px; font-weight: bold;\">{otp}</p>" +
        $"<p> Mã OTP này sẽ hết hạn sau <b>1 phút</b>, vui lòng không chia sẻ với bất kỳ ai.</p>" +
        $"<p> Nếu bạn không yêu cầu mã OTP này, vui lòng bỏ qua email này.</ p >" +
        $"<p> Cảm ơn bạn đã tin dùng Tech Seal.</p>" +
        $"</div>";

            SendMail mail = new SendMail();
            mail.Subject = "Mã OTP từ Tech Seal";
            mail.ReceiverName = employee.FullName;
            mail.ToMail = employee.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMail(mail);
            if (isSuccess != null)
            {
                await Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    otpChange = await _otpGeneratorHelper.GenerateOTP(6);
                });
                return Ok("Đã gửi mã OTP thành công !");
            }
            else
            {
                return BadRequest("Gửi OTP thất bại!");
            }
        }

        [HttpPost("GetOTPForgot")]
        public async Task<ActionResult<string>> GetOTPForgot([FromForm] ForgotPassword forgotPassword)
        {

            var employee = await _employeeSvc.GetByEmail(forgotPassword.Email);
            string otp = await _otpGeneratorHelper.GenerateOTP(6);

            otpForgot = otp;

            string content = $"<h1> Xác Minh Tài Khoản</h1>"+
    $"<p> Xin chào ${employee.FullName},</p>"+
    $"<p>"+
       $"Bạn đã yêu cầu lấy lại mật khẩu của bạn.Đây là mã OTP(Mã Xác Minh) của bạn:"+
    $"</p>"+
    $"<p><strong>${otp}</strong></p>"+
    $"<p>"+
       $" Vui lòng sử dụng mã OTP này để xác minh tài khoản của bạn sau đó mật khẩu mới sẽ được gửi đến email cho bạn. Hãy chắc chắn rằng bạn không tiết lộ mã OTP này cho bất kỳ ai khác."+
    $"</p>"+
    $"<p>"+
       $"Mã OTP này có hiệu lực trong <b>1 phút</b>.Nếu bạn không thực hiện yêu cầu này, hãy liên hệ với chúng tôi."+
    $"</p>"+
    $"<p>"+
        $"Trân trọng,"+
        $"Tech Seal"+
    $"</p>";

            SendMail mail = new SendMail();
            mail.Subject = "Xác Thực Lấy Mật Khẩu Mới";
            mail.ReceiverName = employee.FullName;
            mail.ToMail = employee.Email;
            mail.HtmlContent = content;
            string isSuccess = await _sendMailHelper.SendMail(mail);
            if (isSuccess != null)
            {
                await Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    otpForgot = await _otpGeneratorHelper.GenerateOTP(6);
                });
                return Ok("Đã gửi mã OTP thành công !");
            }
            else
            {
                return BadRequest("Gửi OTP thất bại!");
            }
        }
    }
}
