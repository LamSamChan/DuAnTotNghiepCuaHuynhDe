using QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo_API.Services.CustomerService
{
    public interface ICustomerSvc
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(string cusID);
        Task<string> AddNewAsync(Customer customer);
        Task<string> UpdateAsync(Customer customer);
    }
}
