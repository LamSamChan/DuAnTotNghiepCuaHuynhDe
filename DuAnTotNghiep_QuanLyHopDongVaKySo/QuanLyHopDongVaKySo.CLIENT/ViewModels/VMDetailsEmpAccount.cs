using QuanLyHopDongVaKySo.CLIENT.Models;
using ViewModel = QuanLyHopDongVaKySo_API.ViewModels;
using Model = QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsEmpAccount
    {
        public Employee Employee { get; set; }
        public List<ViewModel.PContractViewModel> PendingContracts { get; set; }
        public List<ViewModel.DContractViewModel> DoneContracts { get; set; }
        public List<Model.Role> Roles { get; set; }
        public List<Model.Position> Positions { get; set; }
    }
}
