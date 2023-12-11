using QuanLyHopDongVaKySo.CLIENT.Models;
using ViewModel = QuanLyHopDongVaKySo_API.ViewModels;
using API = QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDeitalsMinute
    {
        public Models.Employee Employee { get; set; }
        public Models.Customer Customer { get; set; }
        public API.DoneMinute DoneMinute { get; set; }
        public API.PendingMinute PendingMinute { get; set; }
        public API.InstallationRequirement Requirement { get; set; }
        public ViewModel.DContractViewModel DoneContract { get; set; }
    }
}
