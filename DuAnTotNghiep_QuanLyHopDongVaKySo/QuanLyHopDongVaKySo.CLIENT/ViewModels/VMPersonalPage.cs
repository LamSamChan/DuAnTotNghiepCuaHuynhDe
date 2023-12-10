using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMPersonalPage
    {
        public List<Role> Roles { get; set; }
        public List<Position> Positions { get; set; }
        public PutEmployee Employee { get; set; }
        public PFXCertificate PFXCertificate { get; set; }

    }
}
