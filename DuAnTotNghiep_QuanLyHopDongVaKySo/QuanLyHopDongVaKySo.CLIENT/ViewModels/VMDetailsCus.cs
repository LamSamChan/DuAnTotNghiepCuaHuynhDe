using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsCus
    {
        public Customer Customer { get; set; }
        public List<PContractViewModel> PendingContracts { get; set; }
        public List<DContractViewModel> DoneContracts { get; set; }
    }
}
