using API = QuanLyHopDongVaKySo_API.Models;
using VMAPI = QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListContractAwait
    {
        public List<VMAPI.PContractViewModel> PContracts { get; set; }
        public API.PFXCertificate PFXCertificate { get; set; }
    }
}
