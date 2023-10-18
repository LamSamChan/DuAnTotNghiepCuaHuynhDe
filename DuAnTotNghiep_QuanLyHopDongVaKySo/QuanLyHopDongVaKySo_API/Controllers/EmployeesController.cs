using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IRandomPasswordHelper _randomPasswordHelper;
        public EmployeesController(IEmployeeSvc employeeSvc, ISendMailHelper sendMailHelper, IRandomPasswordHelper randomPasswordHelper)
        {
            this._employeeSvc = employeeSvc;
            this._sendMailHelper = sendMailHelper;
            this._randomPasswordHelper = randomPasswordHelper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            return Ok(await _employeeSvc.GetAll());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(string id)
        {
            Employee emp = await _employeeSvc.GetById(id);
            if (emp != null)
            {
                return Ok(emp);
            }
            else
            {
                return BadRequest(emp);
            }
        }
        //Chưa bắt lỗi khi dùng Role Với Positon đang bị ẩn trong table, trùng nhân viên
        [HttpPost("AddNew")]
        public async Task<ActionResult<string>> AddNew(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid();
            string passwordEmp = await _randomPasswordHelper.GeneratePassword(8);
            employee.Password = passwordEmp;
            string temp = "tempơ";
            string isError = await _employeeSvc.AddNew(employee);
            if (isError != null)
            {
                string content =

    $"<p>Xin chào <b>{employee.FullName}</b>,</p>" +
    $"<p>Chúc mừng bạn đã tạo thành công tài khoản trên <b>TechSeal - Contract Management & Digital Signature</b>!</p>" +
    $"<p>Dưới đây là thông tin tài khoản của bạn:</p>" +
    $"<ul> "+ 
        $"<li>Tên đăng nhập: {employee.Email}</li>" +
        $"<li>Mật khẩu: {passwordEmp}</li>" +
    $"</ul> " +
    $"<p>Vui lòng lưu trữ thông tin này một cách an toàn. Hãy thay đổi mật khẩu ngay sau khi đăng nhập lần đầu tiên.</p>" +
    $"<p>Nếu bạn gặp bất kỳ vấn đề hoặc có câu hỏi, hãy liên hệ với chúng tôi tại <b>techseal.digitalsignature@gmail.com Hoặc Liên Hệ: 0339292975.</b></p>" +
    $"<p>Chúng tôi rất hân hạnh vì bạn đã trở thành 1 thành viên của <b>TechSeal - Contract Management & Digital Signature</b> và chúc bạn có một ngày tốt lành!</p> " +
    $"<p>Trân trọng,</p> " +
    $"<p>Tech Seal.</p>";

                SendMail mail  = new SendMail();
                mail.Subject = "Chúc mừng bạn đã tạo thành công tài khoản";
                mail.ReceiverName = employee.FullName;
                mail.ToMail = employee.Email;
                mail.HtmlContent = content;
                string isSuccess = await _sendMailHelper.SendMail(mail);
                if (isSuccess != null)
                {
                    return Ok(isError);
                }
                else
                {
                    return BadRequest(isError);
                }
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<string>> Update(Employee employee)
        {
            string isError = await _employeeSvc.Update(employee);

            if (isError != null)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
