using QuanLyHopDongVaKySo.CLIENT.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsContract
    {
        public Employee Employee { get; set; }
        public Models.Customer Customer { get; set; }
        public DoneMinute DoneMinutes { get; set; }
        public PendingMinute PendingMinute { get; set; }
        public PContractViewModel PendingContracts { get; set; }
        public DContractViewModel DoneContracts { get; set; }
        public string Tille { get; set; }
    }
}
