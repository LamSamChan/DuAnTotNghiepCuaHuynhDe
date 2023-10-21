﻿using Microsoft.AspNetCore.Http;
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

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<string>> Update(Customer customer)
        {
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