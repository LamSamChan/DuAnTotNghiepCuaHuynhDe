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

        public EmployeesController(IEmployeeSvc employeeSvc)
        {
            _employeeSvc = employeeSvc;
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
            employee.EmployeeId = Guid.NewGuid();
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
