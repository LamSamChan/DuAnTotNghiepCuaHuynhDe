using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuanLyHopDongVaKySo_CLIENT.Services.CustomerServices;

namespace QuanLyHopDongVaKySo_CLIENT.Pages.BusinessStaff
{
    public class AddCusModel : PageModel
    {
        public QuanLyHopDongVaKySo_API.Models.PostCustomer Customer { get; set; }

        private readonly ICustomerService _customerService;
        public AddCusModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Xử lý khi nhận yêu cầu POST từ biểu mẫu
            if (ModelState.IsValid)
            {
                _customerService.AddNewCustomer(Customer);
                return RedirectToPage("BusinessStaff/ListCus");
            }

            return Page();
        }
    }
}
