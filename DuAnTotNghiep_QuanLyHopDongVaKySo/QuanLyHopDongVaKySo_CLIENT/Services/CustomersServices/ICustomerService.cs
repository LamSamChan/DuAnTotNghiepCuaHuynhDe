using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo_CLIENT.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerById(string id);
        Task<string> AddNewCustomer(Customer postCustomer);
        Task<string> UpdateCustomer(Customer putCustomer);
       
    }
}
