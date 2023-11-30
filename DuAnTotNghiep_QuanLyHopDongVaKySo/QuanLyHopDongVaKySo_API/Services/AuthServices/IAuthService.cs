using QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Services.AuthServices
{
    public interface IAuthService
    {
        Task<Employee> Login(ViewLogin viewLogin);
        Task<Customer> CustomerVerify(string identification);
    }
}
