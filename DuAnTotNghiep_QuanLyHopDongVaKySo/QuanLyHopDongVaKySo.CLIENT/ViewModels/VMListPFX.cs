using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListPFX
    {
        public List<API.PFXCertificate> PFXCertificates { get; set; }
        public List<API.PFXCertificate> PFXCertificatesATE { get; set; }
        public List<API.PFXCertificate> PFXCertificatesE { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
