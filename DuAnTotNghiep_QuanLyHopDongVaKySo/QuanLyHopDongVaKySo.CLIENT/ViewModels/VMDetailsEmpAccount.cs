using QuanLyHopDongVaKySo.CLIENT.Models;
using API=QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsEmpAccount
    {
        public Employee Employee { get; set; }
        public List<API.PendingContract> PendingContracts { get; set; }
        public List<API.DoneContract> DoneContracts { get; set; }
        public List<API.Role> Roles { get; set; }
        public List<API.Position> Positions { get; set; }
    }
}
