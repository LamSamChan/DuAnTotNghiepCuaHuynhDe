using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerSvc _customerSvc;

        public CustomersController(ICustomerSvc customerSvc)
        {
            _customerSvc = customerSvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
        {
            return Ok(await _customerSvc.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetById(string id)
        {
            Customer cus = await _customerSvc.GetByIdAsync(id);
            if (cus != null)
            {
                return Ok(cus);
            }
            else
            {
                return BadRequest(cus);
            }
        }

        [HttpPost("AddNew")]
        public async Task<ActionResult<string>> AddNew(Customer customer)
        {
            customer.CustomerId = Guid.NewGuid();
            string isError = await _customerSvc.AddNewAsync(customer);
            if (isError != null)
            {
                return Ok(isError);
            }
            else { return BadRequest(isError); }
        }
    }
}
