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
        private readonly IPFXCertificateSvc _pfxCertificate;
        private readonly IEmployeeSvc _employeeSvc;
        private readonly IEncodeHelper _encodeHelper;

        public EmployeesController(IEmployeeSvc employeeSvc ,IPFXCertificateSvc pfxCertificate, IEncodeHelper encodeHelper)
        {
            _employeeSvc = employeeSvc;
            _pfxCertificate = pfxCertificate;
            _encodeHelper = encodeHelper;
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
        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <remarks>
        /// Ví dụ:
        /// {
        ///   "FullName": "Nguyen Van A",
        ///   "Email": "ANguyen@example.com",
        ///   "DateOfBirth": "04/25/2003",
        ///   "Gender": 1,
        ///   "PhoneNumber": "0339292975",
        ///   "Identification": "012345678923",
        ///   "Address": "55 Phan Xich Long P16 Q11",
        ///   "Image": "Đường dẫn ảnh",
        ///   "Password": "mật khẩu",
        ///   "IsLocked": false,
        ///   "Note":"Ghi chú",
        ///   "RoleID": 1,
        ///   "PositionID":2
        /// }
        /// </remarks>
        /// <param name="model">Thông tin đối tượng mới.</param>
        [HttpPost("AddNew")]
        public async Task<ActionResult<string>> AddNew(Employee employee)
        {
            string serialPFX = await _pfxCertificate.CreatePFXCertificate("TechSeal", employee.FullName, _encodeHelper.Encode(employee.Password), true);
            employee.EmployeeId = Guid.NewGuid();
            employee.SerialPFX = serialPFX;
            string isError = await _employeeSvc.AddNew(employee);
            if (isError != null)
            {
                return Ok(isError);
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
