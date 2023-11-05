// Ignore Spelling: auth

using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthServices _authServices;
        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(VMLogin login)
        {
            int reponse = await _authServices.Login(login);
            if (reponse != 0)
            {
                return RedirectToAction("Index", "BusinessStaff");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}
