using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.DoneContractService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DContractController : ControllerBase
    {
        private readonly IDoneContractSvc _doneContractSvc;
        public DContractController(IDoneContractSvc doneContractSvc)
        {
            _doneContractSvc = doneContractSvc;
        }

        [HttpGet("getAllEffect")]
        public async Task<IActionResult> getAllEffect()
        {
            return Ok(await _doneContractSvc.getListIsEffect());
        }
    }
}
