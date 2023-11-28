using QuanLyHopDongVaKySo.CLIENT.Models;
using ViewModel = QuanLyHopDongVaKySo_API.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContract
    {
        public Models.Employee Employee { get; set; }
        public Models.Customer Customer { get; set; }
        public API.DoneMinute DoneMinutes { get; set; }
        public API.PendingMinute PendingMinute { get; set; }
        public ViewModel.PContractViewModel PendingContracts { get; set; }
        public ViewModel.DContractViewModel DoneContracts { get; set; }
        
    }
}
