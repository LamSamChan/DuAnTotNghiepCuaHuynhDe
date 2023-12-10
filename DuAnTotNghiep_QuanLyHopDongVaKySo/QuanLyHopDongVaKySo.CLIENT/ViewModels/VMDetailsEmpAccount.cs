using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.ViewModels;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsEmpAccount
    {
        public Employee Employee { get; set; }
        public List<PContractViewModel> PendingContracts { get; set; }
        public List<DContractViewModel> DoneContracts { get; set; }
        public List<Role> Roles { get; set; }
        public List<Position> Positions { get; set; }
    }
}
