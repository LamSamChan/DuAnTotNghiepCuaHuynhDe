using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryEmpService;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class HistoryEmpController : ControllerBase
    {
        private readonly IOperationHistoryEmpSvc _operationHistoryEmpSvc;

        public HistoryEmpController(IOperationHistoryEmpSvc operationHistoryEmpSvc)
        {
            _operationHistoryEmpSvc = operationHistoryEmpSvc;
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(OperationHistoryEmp emp)
        {
            emp.Date = DateTime.Now;
            int isError = await _operationHistoryEmpSvc.AddNew(emp);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationHistoryEmp>>> GetAll()
        {
            return Ok(await _operationHistoryEmpSvc.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetListById(string id)
        {
            List<OperationHistoryEmp> historyEmps = await _operationHistoryEmpSvc.GetListById(id);
            if (historyEmps != null)
            {
                return Ok(historyEmps);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
