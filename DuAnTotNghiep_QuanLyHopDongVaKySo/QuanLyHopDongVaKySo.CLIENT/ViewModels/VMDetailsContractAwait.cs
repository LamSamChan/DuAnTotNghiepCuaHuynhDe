using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContractAwait
    {
        public API.PendingContract PContract { get; set; }
        public Employee Employee { get; set; }
        public Customer Customer { get; set; }
        public API.PFXCertificate PFXCertificate { get; set; }
    }
}
