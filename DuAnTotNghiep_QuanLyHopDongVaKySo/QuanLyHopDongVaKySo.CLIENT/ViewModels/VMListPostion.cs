using Model = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListPostion
    {
        public List<Model.Position>? Postions { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
