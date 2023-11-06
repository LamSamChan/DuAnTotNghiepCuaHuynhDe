using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMAdminListUsersAccount
    {
        public List<Employee> Employees { get; set; }
        public List<Customer> Customers { get; set; }
        public List<API.Role> Roles { get; set; }
        public List<API.Position> Positions { get; set; }
    }
}
