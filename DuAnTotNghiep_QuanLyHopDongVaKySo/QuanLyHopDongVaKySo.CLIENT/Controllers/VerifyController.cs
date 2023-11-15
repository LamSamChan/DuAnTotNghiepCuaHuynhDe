// Ignore Spelling: auth

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Services;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Constants;
using QuanLyHopDongVaKySo_API.ViewModels;
using QuanLyHopDongVaKySo.CLIENT.Services.PasswordServices;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IAuthServices _authServices;
        private readonly IRoleService _roleService;
        private readonly IPasswordService _passwordService;
        public VerifyController(IAuthServices authServices, IRoleService roleService, IPasswordService passwordService)
        {
            _authServices = authServices;
            _roleService = roleService;
            _passwordService = passwordService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ResetPass()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetOTP([FromBody]ForgotPassword forgotPassword)
        {
            var respone = await _passwordService.GetOTPForgotAsync(forgotPassword);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] string comfirmOTP)
        {
            var respone = await _passwordService.ForgotPasswordAsync(comfirmOTP);
            if (respone != null)
            {
                return Ok();
            }
            return BadRequest();
        }

        public async Task<IActionResult> Login(VMLogin login)
        {
            var reponse = await _authServices.Login(login);
            if (reponse != null)
            {
                HttpContext.Session.SetString(SessionKey.Employee.EmployeeContext,
                        JsonConvert.SerializeObject(reponse));
                HttpContext.Session.SetString(SessionKey.Employee.EmployeeID, reponse.EmployeeId.ToString());
                HttpContext.Session.SetString(SessionKey.Employee.Role, _roleService.GetRoleByIdAsync(reponse.RoleID).Result.RoleName);
                string role = HttpContext.Session.GetString(SessionKey.Employee.Role);
                ViewBag.Role = role;
                if (role == "Admin")
                {
                    /*if (reponse.IsFirstLogin)
                    {
                        return RedirectToAction("ChangePass", "Admin");
                    }*/
                    return RedirectToAction("Index", "Admin");
                }
                else if (role == "Giám đốc")
                {
                    if (reponse.IsFirstLogin)
                    {
                        return RedirectToAction("ChangePass", "Director");
                    }
                    return RedirectToAction("Index", "Director");
                }
                else if (role =="Nhân viên kinh doanh")
                {
                    if (reponse.IsFirstLogin)
                    {
                        return RedirectToAction("ChangePass", "BusinessStaff");
                    }
                    return RedirectToAction("Index", "BusinessStaff");
                }
                else
                {
                    if (reponse.IsFirstLogin)
                    {
                        return RedirectToAction("ChangePass", "InstallStaff");
                    }
                    return RedirectToAction("Index", "InstallStaff");
                } 
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
