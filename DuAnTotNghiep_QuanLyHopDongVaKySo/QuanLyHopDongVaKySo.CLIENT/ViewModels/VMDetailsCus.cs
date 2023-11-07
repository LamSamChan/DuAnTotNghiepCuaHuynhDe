using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsCus
    {
        public Customer Customer { get; set; }
        public List<API.PendingContract> PendingContracts { get; set; }
        public List<API.DoneContract> DoneContracts { get; set; }
    }
}
