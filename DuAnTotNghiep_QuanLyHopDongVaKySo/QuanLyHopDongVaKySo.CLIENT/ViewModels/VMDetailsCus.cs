using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsCus
    {
        public Customer Customer { get; set; }
        public List<API.PContractViewModel> PendingContracts { get; set; }
        public List<API.DContractViewModel> DoneContracts { get; set; }
    }
}
