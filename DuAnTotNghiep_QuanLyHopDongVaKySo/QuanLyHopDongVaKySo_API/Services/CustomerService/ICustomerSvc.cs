using QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo_API.Services.CustomerService
{
    public interface ICustomerSvc
    {
        Task<List<Customer>> getCustomersAsnyc();
        Task<Customer> getCustomerAsnyc(Guid id);
        Task<string> addCustomerAsnyc(PostCustomer customer);
        Task<string> updateCustomerAsnyc(PutCustomer customer);
        Task<bool> deleteCustomerAsnyc(Guid id); 
    }
}
