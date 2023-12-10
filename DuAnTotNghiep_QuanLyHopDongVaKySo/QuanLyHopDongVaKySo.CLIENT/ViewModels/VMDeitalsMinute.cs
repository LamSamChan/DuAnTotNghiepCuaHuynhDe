using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDeitalsMinute
    {
        public Employee Employee { get; set; }
        public Customer Customer { get; set; }
        public DoneMinute DoneMinute { get; set; }
        public PendingMinute PendingMinute { get; set; }
        public InstallationRequirement Requirement { get; set; }
        public DContractViewModel DoneContract { get; set; }
    }
}
