using Azure;
using Microsoft.AspNetCore.Mvc;
using QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo.CLIENT.Services.PContractServices;
using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo_API.ViewModels;
using Newtonsoft.Json;

namespace QuanLyHopDongVaKySo.CLIENT.Controllers
{
    public class BusinessStaffController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IPContractService _pContractService;
        public BusinessStaffController(ICustomerService customerService, IPContractService pContractService) { 
            _customerService = customerService;
            _pContractService = pContractService;
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


        public async Task<IActionResult> AddCusAction(PostCustomer customer)
        {
            customer.IsLocked = false;
            if (customer.BuisinessName != null)
            {
                customer.typeofCustomer = "Doanh nghiệp";
            }
            else
            {
                customer.typeofCustomer = "Cá nhân";
            }
            int reponse = await _customerService.AddNewCustomer(customer);
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
        public async Task<IActionResult> ContractListEffect()
        {
            List<DContractViewModel> contractList = new List<DContractViewModel>();
            try
            {
                contractList = await _pContractService.getListEffect();
                return View(contractList);
            }
            catch (Exception ex)
            {

                return View(contractList);
            }
        }
        public IActionResult ContractListPending()
        {
            return View();
        }
        public IActionResult PersonalPage()
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
        public async Task<IActionResult> EditCus(string customerID)
        {
            var customer = await _customerService.GetCustomerById(customerID);
            if (customer != null)
            {
                return View(customer);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> UpdateInfoCustomer(PutCustomer putCustomer)
        {

            if (putCustomer.BuisinessName != null)
            {
                putCustomer.typeofCustomer = "Doanh nghiệp";
            }
            else
            {
                putCustomer.typeofCustomer = "Cá nhân";
            }

            string respone = await _customerService.UpdateCustomer(putCustomer);
            var customer = await _customerService.GetCustomerById(respone);
            if (customer != null)
            {
                return View("EditCus", customer);
            }
            else
            {
                return RedirectToAction("Index");
            }
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
