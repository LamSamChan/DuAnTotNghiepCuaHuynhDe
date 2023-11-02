using Microsoft.AspNetCore.Mvc;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddPosition()
        {
            return View();
        }
        public IActionResult AddRole()
        {
            return View();
        }
        public IActionResult DetailsAccount()
        {
            return View();
        }
        public IActionResult ListAccount()
        {
            return View();
        }
        public IActionResult ListPosition()
        {
            return View();
        }
        public IActionResult ListRole()
        {
            return View();
        }
    }
}
