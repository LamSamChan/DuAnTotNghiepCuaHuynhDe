using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services.EmployeeService;
using QuanLyHopDongVaKySo_API.Services.RoleService;
using QuanLyHopDongVaKySo_API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyHopDongVaKySo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmployeeSvc _employeeSvc;
        private readonly IRoleSvc _roleSvc;
        private readonly IConfiguration _configuration;
        public AuthController(IEmployeeSvc employeeSvc, IConfiguration configuration, IRoleSvc roleSvc)
        {
            _employeeSvc = employeeSvc;
            _configuration = configuration;
            _roleSvc = roleSvc;
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> Login([FromForm] ViewLogin viewLogin)
        {
            string token = null;
            var login = await _employeeSvc.Login(viewLogin);
            if (login == null)
            {
                return BadRequest("Sai mật khẩu hoặc tài khoản, hãy kiểm tra lại !");
            }
            var role = _roleSvc.GetById(login.RoleID).Result.RoleName;
            token = CreateToken(viewLogin,role);
            return Ok(token);
        }

        private string CreateToken(ViewLogin viewLogin, string role)
        {
            List<Claim> claims = new List<Claim>() { 
                new Claim(ClaimTypes.Name, viewLogin.Email),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["AppSettings:Token"]!));
           
            var creads = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creads
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
