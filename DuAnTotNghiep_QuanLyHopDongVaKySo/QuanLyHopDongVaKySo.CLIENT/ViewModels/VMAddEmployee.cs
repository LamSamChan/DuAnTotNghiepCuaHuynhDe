using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMAddEmployee
    {
        public PostEmployee Employee { get; set; }
        public List<Role> Roles { get; set; }
        public List<Position> Positions { get; set; }
    }
}
