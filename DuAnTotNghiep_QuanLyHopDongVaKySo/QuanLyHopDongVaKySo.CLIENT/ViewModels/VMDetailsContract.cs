using QuanLyHopDongVaKySo.CLIENT.Models;
using ViewModel = QuanLyHopDongVaKySo_API.ViewModels;
using Model = QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContract
    {
        public Employee Employee { get; set; }
        public Customer Customer { get; set; }
        public ViewModel.PContractViewModel PendingContracts { get; set; }
        public ViewModel.DContractViewModel DoneContracts { get; set; }
    }
}
