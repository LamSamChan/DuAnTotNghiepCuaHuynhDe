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


        [HttpGet("CountCustomerAddedByDate")]
        public IActionResult GetNumberCustomer([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _exec.CountCustomerAddedByDate(startDate, endDate);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountCustomerAddedByWeek")]
        public IActionResult GetNumberCustomerWeek([FromQuery] DateTime month)
        {
            var result = _exec.CountCustomerAddedByWeek(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountCustomerAddedByMonth")]
        public IActionResult GetNumberCustomerMonth([FromQuery] DateTime month)
        {
            var result = _exec.CountCustomerAddedByMonth(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }




        //Count PContract
        [HttpGet("CountPContractCreatedByDate")]
        public IActionResult CountPContractCreatedByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _exec.CountPContractByDate(startDate, endDate);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountPContractCreatedByWeek")]
        public IActionResult CountPContractAddedByWeek([FromQuery] DateTime month)
        {
            var result = _exec.CountPContractByWeek(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountPContractCreatedByMonth")]
        public IActionResult CountPContractCreatedByMonth([FromQuery] DateTime month)
        {
            var result = _exec.CountPContractByMonth(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }



        //Count PContract
        [HttpGet("CountPContractWaitCusByDate")]
        public IActionResult CountPContractWaitCusByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _exec.CountPContractWaitCusByDate(startDate, endDate);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountPContractWaitCusByWeek")]
        public IActionResult CountPContractWaitCusByWeek([FromQuery] DateTime month)
        {
            var result = _exec.CountPContractWaitCusByWeek(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountPContractWaitCusByMonth")]
        public IActionResult CountPContractWaitCusByMonth([FromQuery] DateTime month)
        {
            var result = _exec.CountPContractWaitCusByMonth(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }




        //Count DContract
        [HttpGet("CountDContractCreatedByDate")]
        public IActionResult CountDContractCreatedByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _exec.CountDContractByDate(startDate, endDate);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountDContractCreatedByWeek")]
        public IActionResult CountDContractAddedByWeek([FromQuery] DateTime month)
        {
            var result = _exec.CountDContractByWeek(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountDContractCreatedByMonth")]
        public IActionResult CountDContractCreatedByMonth([FromQuery] DateTime month)
        {
            var result = _exec.CountDContractByMonth(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }





        [HttpGet("CountUnEffectByDate")]
        public IActionResult CountUnEffectCreatedByDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _exec.CountUnEffectByDate(startDate, endDate);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountUnEffectByWeek")]
        public IActionResult CountUnEffectByWeek([FromQuery] DateTime month)
        {
            var result = _exec.CountUnEffectByWeek(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

        [HttpGet("CountUnEffectByMonth")]
        public IActionResult CountUnEffectByMonth([FromQuery] DateTime month)
        {
            var result = _exec.CountUnEffectByMonth(month);

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }

    }
}
