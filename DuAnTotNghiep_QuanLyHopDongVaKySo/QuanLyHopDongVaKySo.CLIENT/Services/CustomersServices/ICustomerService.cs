using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
namespace QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(string id);
        Task<string> AddNewCustomer(PostCustomer postCustomer);
        Task<string> UpdateCustomer(PutCustomer putCustomer);
       
    }
}
