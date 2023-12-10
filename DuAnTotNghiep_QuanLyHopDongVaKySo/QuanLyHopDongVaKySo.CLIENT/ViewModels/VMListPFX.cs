using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListPFX
    {
        public List<PFXCertificate> PFXCertificates { get; set; }
        public List<PFXCertificate> PFXCertificatesATE { get; set; }
        public List<PFXCertificate> PFXCertificatesE { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
