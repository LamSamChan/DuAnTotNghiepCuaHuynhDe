using Model = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListRole
    {
        public List<Model.Role>? Roles { get; set; }
        public List<Employee>? Employees { get; set; }
        public Model.Role Role { get; set; }
    }
}
