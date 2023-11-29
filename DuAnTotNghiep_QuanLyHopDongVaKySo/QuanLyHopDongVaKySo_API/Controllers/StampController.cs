using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.StampService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class StampController : Controller
    {
        private readonly IStampSvc _stampSvc;

        public StampController(IStampSvc stampSvc)
        {
            _stampSvc = stampSvc;
;
        }
        [HttpGet]
        public async Task<ActionResult<List<Stamp>>> GetAll()
        {
            return Ok(await _stampSvc.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Stamp>> GetById(int id)
        {
            Stamp stamp = await _stampSvc.GetById(id);
            if (stamp != null)
            {
                return Ok(stamp);
            }
            else
            {
                return BadRequest(stamp);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew([FromBody] Stamp stamp)
        {
            var stamps = await _stampSvc.GetAll();

            if (stamps.Count == 1)
            {
                return BadRequest("Dấu mộc đã tồn tại bạn cần xoá dấu mộc hiện tại để có thể tải ảnh mới lên");
            }

            stamp.DateUpdated = DateTime.Now;
            int isError = await _stampSvc.AddNew(stamp);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<int>> Update([FromBody] Stamp stamp)
        {
            int isError = await _stampSvc.Update(stamp);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            int isError = await _stampSvc.Delete(id);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
