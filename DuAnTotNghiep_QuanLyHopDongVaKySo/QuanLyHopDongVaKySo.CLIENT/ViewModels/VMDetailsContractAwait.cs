using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContractAwait
    {
        public PContractViewModel PContract { get; set; }
        public Employee EmployeeCreated { get; set; }
        public Customer Customer { get; set; }
        public PFXCertificate PFXCertificate { get; set; }
    }
}
