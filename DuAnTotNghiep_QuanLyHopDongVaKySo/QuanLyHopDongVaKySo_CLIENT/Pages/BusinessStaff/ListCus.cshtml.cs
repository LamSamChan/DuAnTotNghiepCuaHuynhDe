using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyHopDongVaKySo_CLIENT.Services.CustomerServices;
using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_CLIENT.Pages.BusinessStaff
{
    public class ListCusModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public ListCusModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public IEnumerable<QuanLyHopDongVaKySo_API.Models.Customer> Customers { get; set; }

        public async Task OnGet()
        {
            try
            {
                Customers = await _customerService.GetAllCustomers();
            }
            catch (Exception ex)
            {
                Customers = new List<QuanLyHopDongVaKySo_API.Models.Customer>();
            }
        }
    }
}
