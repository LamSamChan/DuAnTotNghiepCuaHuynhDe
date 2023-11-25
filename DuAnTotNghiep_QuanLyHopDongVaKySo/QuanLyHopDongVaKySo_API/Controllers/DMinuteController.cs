using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;
using QuanLyHopDongVaKySo_API.Services.DoneMinuteService;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;
using QuanLyHopDongVaKySo_API.Services.PendingMinuteService;
using QuanLyHopDongVaKySo_API.Services.TemplateMinuteService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;
using System.Reflection;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DMinuteController : ControllerBase
    {
        private readonly IDoneMinuteSvc _doneMinuteSvc;

        public DMinuteController(IDoneMinuteSvc doneMinuteSvc)
        {
            _doneMinuteSvc = doneMinuteSvc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            return Ok(await _doneMinuteSvc.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _doneMinuteSvc.GetById(int.Parse(id)));
        }

        [HttpGet("Employee/{id}")]
        public async Task<IActionResult> GetByEmployeeId(string id)
        {
            return Ok(await _doneMinuteSvc.GetListByEmpId(id));
        }
    }
}