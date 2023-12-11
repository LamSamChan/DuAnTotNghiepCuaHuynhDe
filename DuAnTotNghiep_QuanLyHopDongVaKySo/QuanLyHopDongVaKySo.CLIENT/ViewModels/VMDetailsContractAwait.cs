using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo_API.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContractAwait
    {
        public PContractViewModel PContract { get; set; }
        public Employee EmployeeCreated { get; set; }
        public Customer Customer { get; set; }
        public API.PFXCertificate PFXCertificate { get; set; }
    }
}
