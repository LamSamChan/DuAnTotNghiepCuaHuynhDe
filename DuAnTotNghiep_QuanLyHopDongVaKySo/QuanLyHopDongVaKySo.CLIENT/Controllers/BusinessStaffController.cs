using Microsoft.AspNetCore.Mvc;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ILogger<BusinessStaffController> _logger;

        public BusinessStaffController(ILogger<BusinessStaffController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
