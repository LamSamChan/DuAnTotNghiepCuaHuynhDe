﻿using Microsoft.AspNetCore.Authorization;
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

    }
}
