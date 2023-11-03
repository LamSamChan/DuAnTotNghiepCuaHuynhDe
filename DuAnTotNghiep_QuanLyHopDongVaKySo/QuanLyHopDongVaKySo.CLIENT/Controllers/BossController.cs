using Microsoft.AspNetCore.Mvc;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BossController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ListContractAwait()
        {
            return View();
        }
        public IActionResult ListContractActive()
        {
            return View();
        }
        public IActionResult InforSign()
        {
            return View();
        }
        public IActionResult DetailsContractAwait()
        {
            return View();
        }
        public IActionResult DetailsApprovedContract()
        {
            return View();
        }
        public IActionResult DetailsActiveContract()
        {
            return View();
        }
        public IActionResult ListContractEffect()
        {
            return View();
        }
    }
}
