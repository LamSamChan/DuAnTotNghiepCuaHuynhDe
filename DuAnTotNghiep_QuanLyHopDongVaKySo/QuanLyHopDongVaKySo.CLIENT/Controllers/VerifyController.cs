// Ignore Spelling: auth

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QuanLyHopDongVaKySo.CLIENT.Services;
using QuanLyHopDongVaKySo.CLIENT.Services.RoleServices;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
using QuanLyHopDongVaKySo_CLIENT.Constants;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IAuthServices _authServices;
        private readonly IRoleService _roleService;
        public VerifyController(IAuthServices authServices, IRoleService roleService)
        {
            _authServices = authServices;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
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
                    return RedirectToAction("Index", "Admin");
                }
                else if (role == "Giám đốc")
                {
                    return RedirectToAction("Index", "Director");
                }
                else if (role =="Nhân viên kinh doanh")
                {
                    return RedirectToAction("Index", "BusinessStaff");
                }
                else
                {
                    return RedirectToAction("PersonalView", "InstallStaff");
                } 
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
