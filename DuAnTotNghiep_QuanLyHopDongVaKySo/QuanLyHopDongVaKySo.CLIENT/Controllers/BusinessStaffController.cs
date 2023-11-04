using Azure;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ICustomerService _customerService;
        public BusinessStaffController(ICustomerService customerService) { 
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            List<Customer> customersList = new List<Customer>();
            try
            {
                customersList = await _customerService.GetAllCustomers();
                return View(customersList);
            }
            catch (Exception  ex)
            {

                return View(customersList);
            }
        }
        public IActionResult AddCus()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCusAction(PostCustomer postCustomer)
        {
            postCustomer.TOC_ID = 1;
            var gender = postCustomer.Gender;
            int temp = 0;
            int reponse = await _customerService.AddNewCustomer(postCustomer);
            if (reponse != 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
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
        public IActionResult SendMail()
        {
            return View();
        }
    }
}
