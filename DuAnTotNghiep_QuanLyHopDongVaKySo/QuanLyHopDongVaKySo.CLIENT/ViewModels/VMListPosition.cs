using Model = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListPosition
    {
        public List<Model.Position>? Positions { get; set; }
        public List<Employee> Employees { get; set; }

        public Model.Position Position { get; set; }
    }
}
