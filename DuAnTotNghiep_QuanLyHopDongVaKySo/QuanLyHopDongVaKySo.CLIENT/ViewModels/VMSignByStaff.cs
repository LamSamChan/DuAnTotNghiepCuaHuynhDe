using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMSignByStaff
    {
        public PendingMinute PMinute { get; set; }
        public Customer Customer { get; set; }
        public PFXCertificate PFXCertificate { get; set; }
    }
}
