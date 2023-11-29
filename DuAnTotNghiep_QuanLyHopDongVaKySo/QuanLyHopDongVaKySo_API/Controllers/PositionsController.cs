using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.PositionService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PositionsController : ControllerBase
    {
        private readonly IPositionSvc _positionSvc;
        public PositionsController(IPositionSvc position)
        { 
            _positionSvc = position;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> GetAll()
        {
            return Ok(await _positionSvc.GetAll());
        }


        [HttpGet("NotHidden")]
        public async Task<ActionResult<IEnumerable<Position>>> GetAllNotHidden()
        {
            return Ok(await _positionSvc.GetAllNotHidden());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetById(int id)
        {
            Position position = await _positionSvc.GetById(id);
            if(position != null) 
            {
                return Ok(position);
            }
            else
            {
                return BadRequest(position);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(Position position)
        {
            position.isHidden = false;
            int isError = await _positionSvc.AddNew(position);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<int>> Update(Position position)
        {
            int isError = await _positionSvc.Update(position);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
