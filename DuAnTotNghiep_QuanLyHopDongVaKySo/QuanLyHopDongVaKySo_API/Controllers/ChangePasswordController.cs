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
    public class ChangePasswordController : ControllerBase
    {
        private static OneTimePassworrd OTP = new OneTimePassworrd();
        private readonly IEmployeeSvc _employeeSvc;
        private readonly IOTPGeneratorHelper _otpGeneratorHelper;
        private readonly ISendMailHelper _sendMailHelper;
        public ChangePasswordController(IEmployeeSvc employeeSvc, IOTPGeneratorHelper otpGeneratorHelper, ISendMailHelper sendMailHelper) { 
            _employeeSvc = employeeSvc;
            _otpGeneratorHelper = otpGeneratorHelper;
            _sendMailHelper = sendMailHelper;
        }

        [HttpPost]
        public async Task<ActionResult<string>> ChangePassword([FromForm] ChangePassword changePassword, string employeeId, string comfirmOTP)
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

        [HttpPost("GetOTP")]
        public async Task<ActionResult<string>> GetOTP(string employeeId)
        {

            var employee = await _employeeSvc.GetById(employeeId);
            string otp = _otpGeneratorHelper.GenerateOTP(8).Result;

            OTP.Otp = otp;

            string content = $"<div style = \"font-family: Arial, sans-serif; padding: 20px; \">" +
        $"<h2> Xác thực bằng Mã OTP từ Tech Seal </h2>" + 
        $"<p> Cảm ơn bạn đã sử dụng dịch vụ của Tech Seal.</p>" +
        $"<p> Dưới đây là mã OTP của bạn:</p>" +
        $"<p style=\"font-size: 24px; font-weight: bold;\">{otp}</p>" +
        $"<p> Mã OTP này sẽ hết hạn sau một thời gian ngắn, vui lòng không chia sẻ với bất kỳ ai.</p>" +
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
                await Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    OTP.Otp = null;
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
