using QuanLyHopDongVaKySo.CLIENT.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsMinute
    {
        public DoneMinute DoneMinute { get; set; }
        public PendingMinute PendingMinute { get; set; }
        public Models.Employee Employee { get; set; }
        public Models.Customer Customer { get; set; }
    }
}
