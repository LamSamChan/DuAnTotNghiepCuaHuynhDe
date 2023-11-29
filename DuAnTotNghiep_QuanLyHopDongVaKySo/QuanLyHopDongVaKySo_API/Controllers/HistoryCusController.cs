using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.OperationHistoryCusService;
using Microsoft.AspNetCore.Authorization;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class HistoryCusController : ControllerBase
    {
        private readonly IOperationHistoryCusSvc _operationHistoryCusSvc;

        public HistoryCusController(IOperationHistoryCusSvc operationHistoryCusSvc)
        {
            _operationHistoryCusSvc = operationHistoryCusSvc;
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(OperationHistoryCus cus)
        {
            cus.Date = DateTime.Now;
            int isError = await _operationHistoryCusSvc.AddNew(cus);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperationHistoryCus>>> GetAll()
        {
            return Ok(await _operationHistoryCusSvc.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetListById(string id)
        {
            List<OperationHistoryCus> historyCus = await _operationHistoryCusSvc.GetListById(id);
            if (historyCus != null)
            {
                return Ok(historyCus);
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
