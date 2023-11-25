using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListInstallRecord
    {
        public List<PendingMinute> pendingMinutes { get; set; }
        public List<DoneMinute> doneMinutes { get; set; }
        public List<DoneContract> DContracts { get; set; }
    }
}
