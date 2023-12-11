using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMSignByStaff
    {
        public API.PendingMinute PMinute { get; set; }
        public Customer Customer { get; set; }
        public API.PFXCertificate PFXCertificate { get; set; }
    }
}
