using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
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
        public async Task<IActionResult> GetAllEffect()
        {
            return Ok(await _doneContractSvc.getListIsEffect());
        }

        [HttpGet("getByCustomerId/{id}")]
        public async Task<IActionResult> GetbyCusId(string id)
        {
            return Ok(await _doneContractSvc.getListByCusId(id));
        }
        [HttpGet("getByEmpId/{id}")]
        public async Task<IActionResult> GetbyEmpId(string id)
        {
            return Ok(await _doneContractSvc.getListByEmpId(id));
        }
        [HttpGet("getByDirectorId/{id}")]
        public async Task<IActionResult> GetbyDirectorId(string id)
        {
            return Ok(await _doneContractSvc.getListByDirectorId(id));
        }

        [HttpPut]
        public async Task<IActionResult> Update(PutDContract dContract)
        {
            return Ok (await _doneContractSvc.updateAsnyc(dContract));
        }
    }
}
