using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
namespace QuanLyHopDongVaKySo.CLIENT.Services.CustomerServices
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<PutCustomer> GetCustomerByIdPut(string id);
        Task<Customer> GetCustomerById(string id);

        Task<int> AddNewCustomer(PostCustomer postCustomer);
        Task<string> UpdateCustomer(PutCustomer putCustomer);
       
    }
}
