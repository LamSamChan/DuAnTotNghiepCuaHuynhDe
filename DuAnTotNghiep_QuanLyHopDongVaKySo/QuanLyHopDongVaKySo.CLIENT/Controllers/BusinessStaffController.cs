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
        public IActionResult AddCus()
        {
            return View();
        }
        public IActionResult ContractFormPage()
        {
            return View();
        }
        public IActionResult ContractListEffect()
        {
            return View();
        }
        public IActionResult ContractListPending()
        {
            return View();
        }
        public IActionResult ContractListRefuse()
        {
            return View();
        }
        public IActionResult ContractListWaitSign()
        {
            return View();
        }
        public IActionResult CreateFormContract()
        {
            return View();
        }
        public IActionResult DetailsContractEffect()
        {
            return View();
        }
        public IActionResult DetailsContractPending()
        {
            return View();
        }
        public IActionResult DetailsContractRefuse()
        {
            return View();
        }
        public IActionResult DetailsContractWaitSign()
        {
            return View();
        }
        public IActionResult DetailsCus()
        {
            return View();
        }
        public IActionResult HistoryOperation()
        {
            return View();
        }
        public IActionResult ListCus()
        {
            return View();
        }
        public IActionResult SendMail()
        {
            return View();
        }
    }
}
