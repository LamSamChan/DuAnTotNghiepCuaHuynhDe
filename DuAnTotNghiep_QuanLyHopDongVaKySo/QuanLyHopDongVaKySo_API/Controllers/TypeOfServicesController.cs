using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.Services.CustomerService;
using QuanLyHopDongVaKySo_API.Services.TypeOfServiceService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return BadRequest();
            }
        }

        //bắt lỗi mỗi dịch vụ chỉ có 1 hợp đồng và 1 biên bản
        [HttpPost("AddNew")]
        public async Task<ActionResult<int>> AddNew([FromBody] PostTOS typeOfService)
        {
            TypeOfService tos = new TypeOfService() { 
                DateAdded = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                ServiceName = typeOfService.ServiceName,
                Price = typeOfService.Price,
                PerTime = typeOfService.PerTime,
                templateContractID = typeOfService.TContractID,
                isHidden = false,
                templateMinuteID = typeOfService.TMinuteID

            };
            int isError = await _typeOfServiceSvc.AddNew(tos);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<int>> Update([FromBody] PutTOS typeOfService)
        {

            TypeOfService tos = new TypeOfService()
            {
                TOS_ID = typeOfService.TOS_ID,
                DateAdded = typeOfService.DateAdded,
                DateUpdated = DateTime.UtcNow,
                ServiceName = typeOfService.ServiceName,
                Price = typeOfService.Price,
                PerTime = typeOfService.PerTime,
                templateContractID = typeOfService.TContractID,
                isHidden = typeOfService.isHidden,
                templateMinuteID = typeOfService.TMinuteID
            };

            int isError = await _typeOfServiceSvc.Update(tos);
            if (isError != 0)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
