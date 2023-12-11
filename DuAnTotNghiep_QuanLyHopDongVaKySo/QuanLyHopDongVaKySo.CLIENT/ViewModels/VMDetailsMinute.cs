using API = QuanLyHopDongVaKySo_API.Models;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMDetailsMinute
    {
        public API.DoneMinute DoneMinute { get; set; }
        public API.PendingMinute PendingMinute { get; set; }
        public Models.Employee Employee { get; set; }
        public Models.Customer Customer { get; set; }
    }
}
