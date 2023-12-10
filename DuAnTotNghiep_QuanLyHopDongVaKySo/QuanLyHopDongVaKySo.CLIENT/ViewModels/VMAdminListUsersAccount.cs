using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMAdminListUsersAccount
    {
        public List<Employee> Employees { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Role> Roles { get; set; }
        public List<Position> Positions { get; set; }
    }
}
