using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Services.ExecProcedureService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExecProcedureController : ControllerBase
    {
        private readonly IExecProcedureServices _exec;
        public ExecProcedureController(IExecProcedureServices exec)
        {
            _exec = exec;
        }


        [HttpGet("CountCustomerAdded")]
        public IActionResult GetNumberCustomer()
        {
            var result = _exec.CountCustomerAdded();

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

    }
}
