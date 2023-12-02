using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private static string otpChange = String.Empty;
        private static string otpForgot = String.Empty;
        private static string emailEmp = String.Empty;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly IOTPGeneratorHelper _otpGeneratorHelper;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IRandomPasswordHelper _randomPasswordHelper;

        public PasswordController(IEmployeeSvc employeeSvc, IOTPGeneratorHelper otpGeneratorHelper, ISendMailHelper sendMailHelper, IRandomPasswordHelper randomPasswordHelper)
        {
            _employeeSvc = employeeSvc;
            _otpGeneratorHelper = otpGeneratorHelper;
            _sendMailHelper = sendMailHelper;
            _randomPasswordHelper = randomPasswordHelper;
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<string>> ChangePassword([FromBody] ChangePassword changePassword)
        {
            if (otpChange == changePassword.ComfirmOTP || changePassword.EmployeeID != null)
            {
                int? result = await _employeeSvc.ChangePassword(changePassword);
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


        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] string comfirmOTP)
        {
            if (otpForgot == comfirmOTP)
            {
                string newPassword = await _randomPasswordHelper.GeneratePassword(8);
                int result = await _employeeSvc.ForgotPassword(newPassword, emailEmp);
                var employee = await _employeeSvc.GetByEmail(emailEmp);
                if (result == 1)
                {
                    string content = $"<h1> Thiết lập Mật khẩu Mới</h1>" +
                                     $"<p> Xin chào {employee.FullName},</p>" +
                                     $"<p>" +
                                        $"Bạn (hoặc một người nào đó) đã yêu cầu thiết lập lại mật khẩu của bạn. Dưới đây là mật khẩu mới của bạn:" +
                                    $"</p>" +
                                    $"<h3>{newPassword}</h3>" +
                                    $"<p>" +
                                        $"Đừng quên thay đổi mật khẩu này ngay sau khi bạn đăng nhập. Nếu bạn không thực hiện yêu cầu này, hãy liên hệ với chúng tôi ngay lập tức." +
                                    $"</p>" +
                                    $"<p>" +
                                        $"Trân trọng," +
                                    $"</p>"+
                                    $"<p>" +
                                       $"Tech Seal" +
                                    $"</p>";

                    SendMail mail = new SendMail();
                    mail.Subject = "Lấy Mật khẩu Mới";
                    mail.ReceiverName = employee.FullName;
                    mail.ToMail = employee.Email;
                    mail.HtmlContent = content;
                    string isSuccess = await _sendMailHelper.SendMail(mail);
                    if (isSuccess != null)
                    {
                        return Ok("Đã cấp mật khẩu mới");
                    }
                    else
                    {
                        return BadRequest("Cấp mật khẩu mới thất bại");
                    }
                    
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

        [Authorize]
        [HttpPost("GetOTPChange")]
        public async Task<ActionResult<string>> GetOTPChange([FromBody] string employeeId)
        {
            var employee = await _employeeSvc.GetById(employeeId);
            string otp = await _otpGeneratorHelper.GenerateOTP(6);

            otpChange = otp;

            string content = $"<div style = \"font-family: Arial, sans-serif; padding: 20px; \">" +
        $"<h2> Xác thực bằng Mã OTP từ Tech Seal </h2>" +
        $"<p> Cảm ơn bạn đã sử dụng dịch vụ của Tech Seal.</p>" +
        $"<p> Dưới đây là mã OTP của bạn:</p>" +
        $"<p style=\"font-size: 24px; font-weight: bold;\">{otp}</p>" +
        $"<p> Mã OTP này sẽ hết hạn sau <b>3 phút</b>, vui lòng không chia sẻ với bất kỳ ai.</p>" +
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
                    await Task.Delay(TimeSpan.FromMinutes(3));
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
        public async Task<ActionResult<string>> GetOTPForgot([FromBody] ForgotPassword forgotPassword)
        {
            var employee = await _employeeSvc.GetByEmail(forgotPassword.Email);

            if (employee == null)
            {
                return BadRequest("Gửi OTP thất bại!");
            }

            string otp = await _otpGeneratorHelper.GenerateOTP(6);
            emailEmp = employee.Email;
            otpForgot = otp;

            string content = $"<h1> Xác Minh Tài Khoản</h1>" +
    $"<p> Xin chào {employee.FullName},</p>" +
    $"<p>" +
       $"Bạn đã yêu cầu lấy lại mật khẩu của bạn.Đây là mã OTP(Mã Xác Minh) của bạn:" +
    $"</p>" +
    $"<h3>{otp}</h3>" +
    $"<p>" +
       $" Vui lòng sử dụng mã OTP này để xác minh tài khoản của bạn sau đó mật khẩu mới sẽ được gửi đến email cho bạn. Hãy chắc chắn rằng bạn không tiết lộ mã OTP này cho bất kỳ ai khác." +
    $"</p>" +
    $"<p>" +
       $"Mã OTP này có hiệu lực trong <b>3 phút</b>.Nếu bạn không thực hiện yêu cầu này, hãy liên hệ với chúng tôi." +
    $"</p>" +
    $"<p>" +
        $"Trân trọng," +
    $"</p>"+
    $"<p>"+
        $"Tech Seal" +
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
                    await Task.Delay(TimeSpan.FromMinutes(3));
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