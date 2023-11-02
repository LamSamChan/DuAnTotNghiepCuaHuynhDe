using Microsoft.AspNetCore.Mvc;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CusToSign()
        {
            return View();
        }
    }
}
