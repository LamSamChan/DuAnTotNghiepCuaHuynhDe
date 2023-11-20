using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMAdminIndex
    {
        public List<API.Role> Roles { get; set; }
        public List<API.Position> Positions { get; set; }
        public List<API.Stamp> Stamps { get; set; }
        public API.Stamp Stamp { get; set; }
        public PutEmployee Employee { get; set; }
        public API.PFXCertificate PFXCertificate { get; set; }
    }
}
