using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Services.InstallationRequirementService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class IRequirementsController : ControllerBase
    {
        private readonly IInstallationRequirementSvc _service;
        public IRequirementsController(IInstallationRequirementSvc service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() { 
        
            var requirementList = await _service.GetAll();
            if (requirementList != null)
            {
                return Ok(requirementList);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
