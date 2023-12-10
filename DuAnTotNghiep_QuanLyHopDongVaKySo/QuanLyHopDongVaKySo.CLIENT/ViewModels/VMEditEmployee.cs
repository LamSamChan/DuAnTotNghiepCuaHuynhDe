using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;
using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMEditEmployee
    {
        public PutEmployee Employee { get; set; }
        public List<Role> Roles { get; set; }
        public List<Position> Positions { get; set; }
    }
}
