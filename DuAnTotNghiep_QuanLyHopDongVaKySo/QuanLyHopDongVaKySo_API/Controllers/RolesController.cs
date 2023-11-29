using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.RoleService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RolesController : ControllerBase
    {
        private readonly IRoleSvc _roleSvc;
        public RolesController(IRoleSvc role)
        {
            _roleSvc = role;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAll()
        {
            return Ok(await _roleSvc.GetAll());
        }

        [HttpGet("NotHidden")]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllNotHidden()
        {
            return Ok(await _roleSvc.GetAllNotHidden());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetById(int id)
        {
            Role role = await _roleSvc.GetById(id);
            if (role != null)
            {
                return Ok(role);
            }
            else
            {
                return BadRequest(role);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(Role role)
        {
            role.isHidden = false;
            int isError = await _roleSvc.AddNew(role);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<int>> Update(Role role)
        {
            int isError = await _roleSvc.Update(role);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
