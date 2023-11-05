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
                HttpContext.Session.SetString(SessionKey.Employee.EmployeeID,
                        JsonConvert.SerializeObject(reponse.EmployeeId));
                HttpContext.Session.SetString(SessionKey.Employee.Role,
                        JsonConvert.SerializeObject(_roleService.GetRoleByIdAsync(reponse.RoleID).Result.RoleName));
                
                return RedirectToAction("Index", "BusinessStaff");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
