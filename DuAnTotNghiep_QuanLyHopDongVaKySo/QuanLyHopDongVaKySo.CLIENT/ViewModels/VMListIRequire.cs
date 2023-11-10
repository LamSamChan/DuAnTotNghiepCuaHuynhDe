using QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListIRequire
    {
        public List<InstallationRequirement> IRequirements { get; set; }
        public List<DoneContract> DContracts { get; set; }
        public List<PendingMinute> PMinutes { get; set; }
    }
}
