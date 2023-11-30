// Ignore Spelling: Auth

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using QuanLyHopDongVaKySo_API.Database;
using QuanLyHopDongVaKySo_API.Helpers;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Services;
using QuanLyHopDongVaKySo_API.Services.AuthServices;
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
        private readonly IAuthService _authService;
        private readonly IRoleSvc _roleSvc;
        private readonly IConfiguration _configuration;
        private readonly IEncodeHelper _encodeHelper;
        public AuthController(IAuthService authService, IConfiguration configuration, IRoleSvc roleSvc, IEncodeHelper encodeHelper)
        {
            _authService = authService;
            _configuration = configuration;
            _roleSvc = roleSvc;
            _encodeHelper = encodeHelper;
        }

        [HttpPost]
        public async Task<ActionResult> Login(ViewLogin viewLogin)
        {
            string token = null;
            var login = await _authService.Login(viewLogin);
            if (login == null)
            {
                return BadRequest("Sai mật khẩu hoặc tài khoản, hãy kiểm tra lại !");
            }
            var role = _roleSvc.GetById(login.RoleID).Result.RoleName;
            token = CreateToken(viewLogin, role);

            return Ok(token);
        }

        [HttpGet("CustomerVerify/{identification}")]
        public async Task<ActionResult> CustomerVerify(string identification)
        {
            string token = null;
            var login = await _authService.CustomerVerify(identification);
            if (login == null)
            {
                return BadRequest(null);
            }
            token = CreateTokenForCustomer(login.CustomerId.ToString());
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

        private string CreateTokenForCustomer(string idCustomer)
        {
            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, idCustomer),
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
