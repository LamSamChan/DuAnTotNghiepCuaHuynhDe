using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.PositionService;
using QuanLyHopDongVaKySo_API.Services.TypeOfCustomerService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeOfCustomersController : ControllerBase
    {
        private readonly ITypeOfCustomerSvc _typeOfCusSvc;
        public TypeOfCustomersController(ITypeOfCustomerSvc typeOfCusSvc)
        {
            _typeOfCusSvc = typeOfCusSvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOfCustomer>>> GetAll()
        {
            return Ok(await _typeOfCusSvc.GetAll());
        }

        [HttpGet("NotHidden")]
        public async Task<ActionResult<IEnumerable<TypeOfCustomer>>> GetAllNotHidden()
        {
            return Ok(await _typeOfCusSvc.GetAllNotHidden());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeOfCustomer>> GetById(int id)
        {
            TypeOfCustomer typeOfCustomer = await _typeOfCusSvc.GetById(id);
            if (typeOfCustomer != null)
            {
                return Ok(typeOfCustomer);
            }
            else
            {
                return BadRequest(typeOfCustomer);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew(TypeOfCustomer typeOfCustomer)
        {
            int isError = await _typeOfCusSvc.AddNew(typeOfCustomer);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<int>> Update(TypeOfCustomer typeOfCustomer)
        {
            int isError = await _typeOfCusSvc.Update(typeOfCustomer);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
