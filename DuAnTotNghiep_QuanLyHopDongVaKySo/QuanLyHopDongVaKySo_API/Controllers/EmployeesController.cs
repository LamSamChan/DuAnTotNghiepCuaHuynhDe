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

        [HttpPost("AddNew")]
        public async Task<ActionResult<string>> AddNew(Employee employee)
        {
            _pfxCertificate.CreatePFXCertificate("TechSeal", employee.FullName, _encodeHelper.Encode(employee.Password), true);
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
