using VMAPI= QuanLyHopDongVaKySo_API.ViewModels;
namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMContractsActive
    {
        public List<VMAPI.DContractViewModel> DContractsInEffect { get; set; } = new List<VMAPI.DContractViewModel>();
        public List<VMAPI.DContractViewModel> DContractsNotInEffect { get; set; } = new List<VMAPI.DContractViewModel>();
    }
}
