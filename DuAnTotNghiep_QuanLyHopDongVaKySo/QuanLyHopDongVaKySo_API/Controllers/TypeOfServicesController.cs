using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.TypeOfCustomerService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfServicesController : ControllerBase
    {
        private readonly ITypeOfServiceSvc _typeOfServiceSvc;
        public TypeOfServicesController(ITypeOfServiceSvc typeOfServiceSvc)
        {
            _typeOfServiceSvc = typeOfServiceSvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOfService>>> GetAll()
        {
            return Ok(await _typeOfServiceSvc.GetAll());
        }

        [HttpGet("NotHidden")]
        public async Task<ActionResult<IEnumerable<TypeOfService>>> GetAllNotHidden()
        {
            return Ok(await _typeOfServiceSvc.GetAllNotHidden());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeOfService>> GetById(int id)
        {
            TypeOfService typeOfService = await _typeOfServiceSvc.GetById(id);
            if (typeOfService != null)
            {
                return Ok(typeOfService);
            }
            else
            {
                return BadRequest(typeOfService);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(TypeOfService typeOfService)
        {
            int isError = await _typeOfServiceSvc.AddNew(typeOfService);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<int>> Update(TypeOfService typeOfService)
        {
            int isError = await _typeOfServiceSvc.Update(typeOfService);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
