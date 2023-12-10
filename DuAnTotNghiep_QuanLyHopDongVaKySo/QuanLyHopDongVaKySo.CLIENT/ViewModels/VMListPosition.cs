
using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListPosition
    {
        public List<Position>? Positions { get; set; }
        public List<Employee> Employees { get; set; }

        public Position Position { get; set; }
    }
}
