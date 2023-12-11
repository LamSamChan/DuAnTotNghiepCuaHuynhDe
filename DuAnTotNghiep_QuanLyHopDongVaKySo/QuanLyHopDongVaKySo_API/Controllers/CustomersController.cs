using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.CustomerService;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

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

        [HttpGet("Identification/{identification}")]
        public async Task<ActionResult<Customer>> GetByIdentification(string identification)
        {
            Customer cus = await _customerSvc.GetByIdentificationAsync(identification);
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
        public async Task<ActionResult<string>> AddNew(PostCustomer postCustomer)
        {
            Customer customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                DateAdded = postCustomer.DateAdded,
                BuisinessName = postCustomer.BuisinessName,
                FullName = postCustomer.FullName,
                Position = postCustomer.Position,
                DateOfBirth = postCustomer.DateOfBirth,
                Gender = postCustomer.Gender,
                PhoneNumber = postCustomer.PhoneNumber,
                Email = postCustomer.Email,
                PowerOfAttorneyNum = postCustomer.PowerOfAttorneyNum,
                DatePOA = postCustomer.DatePOA,
                WhoPOA = postCustomer.WhoPOA,
                BuisinessNumber = postCustomer.BuisinessNumber,
                BNDate = postCustomer.BNDate,
                BNPlace = postCustomer.BNPlace,
                Identification = postCustomer.Identification,
                IssuedDate = postCustomer.IssuedDate,
                IssuedPlace = postCustomer.IssuedPlace,
                Nationality = postCustomer.Nationality,
                BankAccount = postCustomer.BankAccount,
                BankName = postCustomer.BankName,
                TaxIDNumber = postCustomer.TaxIDNumber,
                FAX = postCustomer.FAX,
                Address = postCustomer.Address,
                ChargeNoticeAddress = postCustomer.ChargeNoticeAddress,
                BillingAddress = postCustomer.BillingAddress,
                IsLocked = postCustomer.IsLocked,
                Note = postCustomer.Note,
                typeofCustomer = postCustomer.typeofCustomer
            };
            
            string isError = await _customerSvc.AddNewAsync(customer);
            if (isError != null && isError != "-1" && isError != "-2" && isError != "-3" && isError != "-4" && isError != "-5" && isError != "1")
            {
                return Ok(isError);

            }else if(isError == "-1")
            {
                return BadRequest("Email khách hàng này đã tồn tại !");
            }
            else if (isError == "-2")
            {
                return BadRequest("Số điện thoại khách hàng này đã tồn tại !");
            }
            else if (isError == "-3")
            {
                return BadRequest("Chứng minh nhân dân / Căn cước công dân của khách hàng này đã tồn tại !");
            }
            else if (isError == "-4")
            {
                return BadRequest("Tài khoản ngân hàng của khách hàng này đã tồn tại !");
            }
            else if (isError == "-5")
            {
                return BadRequest("Mã số thuế của khách hàng này đã tồn tại !");
            }
            else if (isError == "1")
            {
                return BadRequest("Tên loại khách hàng không tồn tại hoặc đã bị ẩn!");
            }
            else { return BadRequest(isError); }
        }

        [HttpPut("Update")]
        public async Task<ActionResult<string>> Update(PutCustomer putCustomer)
        {
            Customer customer = new Customer {
                CustomerId = putCustomer.CustomerId,
                BuisinessName = putCustomer.BuisinessName,
                FullName = putCustomer.FullName,
                Position = putCustomer.Position,
                DateOfBirth = putCustomer.DateOfBirth,
                Gender = putCustomer.Gender,
                PhoneNumber = putCustomer.PhoneNumber,
                Email = putCustomer.Email,
                Identification = putCustomer.Identification,
                IssuedDate = putCustomer.IssuedDate,
                IssuedPlace = putCustomer.IssuedPlace,
                Nationality = putCustomer.Nationality,
                BankAccount = putCustomer.BankAccount,
                BankName = putCustomer.BankName,
                TaxIDNumber = putCustomer.TaxIDNumber,
                Address = putCustomer.Address,
                ChargeNoticeAddress = putCustomer.ChargeNoticeAddress,
                BillingAddress = putCustomer.BillingAddress,
                IsLocked = putCustomer.IsLocked,
                Note = putCustomer.Note,
                typeofCustomer = putCustomer.typeofCustomer
            };
            string isError = await _customerSvc.UpdateAsync(customer);

            if (isError != null && isError != "-1" && isError != "-2" && isError != "-3" && isError != "-4" && isError != "-5" && isError != "1")
            {
                return Ok(isError);

            }
            else if (isError == "-1")
            {
                return BadRequest("Email khách hàng này đã tồn tại !");
            }
            else if (isError == "-2")
            {
                return BadRequest("Số điện thoại khách hàng này đã tồn tại !");
            }
            else if (isError == "-3")
            {
                return BadRequest("Chứng minh nhân dân / Căn cước công dân của khách hàng này đã tồn tại !");
            }
            else if (isError == "-4")
            {
                return BadRequest("Tài khoản ngân hàng của khách hàng này đã tồn tại !");
            }
            else if (isError == "-5")
            {
                return BadRequest("Mã số thuế của khách hàng này đã tồn tại !");
            }
            else if (isError == "1")
            {
                return BadRequest("Tên loại khách hàng không tồn tại hoặc đã bị ẩn!");
            }
            else { return BadRequest(isError); }
        }
    }
}
