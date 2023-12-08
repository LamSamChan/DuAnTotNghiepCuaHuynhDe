using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.PFXCertificateService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeSvc _employeeSvc;
        private readonly ISendMailHelper _sendMailHelper;
        private readonly IRandomPasswordHelper _randomPasswordHelper;
        private readonly IUploadFileHelper _uploadFileHelper;
        public EmployeesController(IEmployeeSvc employeeSvc, ISendMailHelper sendMailHelper, IRandomPasswordHelper randomPasswordHelper, IUploadFileHelper uploadFileHelper)
        {
            this._employeeSvc = employeeSvc;
            this._sendMailHelper = sendMailHelper;
            this._randomPasswordHelper = randomPasswordHelper;
            _uploadFileHelper = uploadFileHelper;
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

        [HttpGet("GetByEmail/{email}")]
        public async Task<ActionResult<Employee>> GetByEmail(string email)
        {
            Employee emp = await _employeeSvc.GetByEmail(email);
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
        public async Task<ActionResult<string>> AddNew(PostEmployee postEmployee)
        {
            
            string passwordEmp = await _randomPasswordHelper.GeneratePassword(8);
            Employee employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FullName = postEmployee.FullName,
                Email = postEmployee.Email,
                DateOfBirth = postEmployee.DateOfBirth,
                Gender = postEmployee.Gender,
                PhoneNumber = postEmployee.PhoneNumber,
                Identification = postEmployee.Identification,
                Address = postEmployee.Address,
                Password = passwordEmp,
                IsFirstLogin = true,
                Image = postEmployee.Image,
                IsLocked = false,
                Note = postEmployee.Note,
                RoleID = postEmployee.RoleID,
                PositionID = postEmployee.PositionID,
        };
       

            string isError = await _employeeSvc.AddNew(employee);

            //-1: email trùng, -2: số điện thoại trùng, -3 cccd trùng
            if (isError != "-1" && isError != "-2" && isError != "-3" && isError != "-4" && isError != "-5" && isError != "-6" && isError != null)
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

                Task.Run(() => _sendMailHelper.SendMail(mail));

                return Ok(isError);

            }
            else {
                if (isError == "-1")
                {
                    return BadRequest("Email nhân viên này đã tồn tại !");

                }else if(isError == "-2")
                {
                    return BadRequest("Số điện thoại nhân viên này đã tồn tại !");
                }
                else if (isError == "-3")
                {
                    return BadRequest("Chứng minh nhân dân / Căn cước công dân của nhân viên này đã tồn tại !");
                }
                else if (isError == "-4")
                {
                    return BadRequest("Vai trò này đã bị ẩn!");
                }
                else if (isError == "-5")
                {
                    return BadRequest("Chức vụ này đã bị ẩn !");
                }
                else if (isError == "-6")
                {
                    return BadRequest("Chức vụ hoặc vai trò không tồn tại!");
                }
                else return BadRequest();
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<string>> Update(PutEmployee putEmployee)
        {
            Employee employee = new Employee {
                EmployeeId = putEmployee.EmployeeId,
                FullName = putEmployee.FullName,
                Email = putEmployee.Email,
                DateOfBirth = putEmployee.DateOfBirth,
                Gender = putEmployee.Gender,
                PhoneNumber = putEmployee.PhoneNumber,
                Identification = putEmployee.Identification,
                Image = putEmployee.Image,
                Address = putEmployee.Address,
                IsLocked = putEmployee.IsLocked,
                IsFirstLogin = putEmployee.IsFirstLogin,
                Note = putEmployee.Note,
                RoleID = putEmployee.RoleID,
                PositionID = putEmployee.PositionID,
            };

            string isError = await _employeeSvc.Update(employee);

            //-1: email trùng, -2: số điện thoại trùng, -3 cccd trùng
            if (isError != "-1" && isError != "-2" && isError != "-3" && isError != "-4" && isError != "-5" && isError != "-6" && isError != null)
            {
                return Ok(isError);
            }
            else
            {
                if (isError == "-1")
                {
                    return BadRequest("Email nhân viên này đã tồn tại !");

                }
                else if (isError == "-2")
                {
                    return BadRequest("Số điện thoại nhân viên này đã tồn tại !");
                }
                else if (isError == "-3")
                {
                    return BadRequest("Chứng minh nhân dân / Căn cước công dân của nhân viên này đã tồn tại !");
                }
                else if (isError == "-4")
                {
                    return BadRequest("Vai trò này đã bị ẩn!");
                }
                else if (isError == "-5")
                {
                    return BadRequest("Chức vụ này đã bị ẩn !");
                }
                else if (isError == "-6")
                {
                    return BadRequest("Chức vụ hoặc vai trò không tồn tại!");
                }
                else return BadRequest(isError);
            }
        }
    }
}
