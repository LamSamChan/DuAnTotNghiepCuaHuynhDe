using QuanLyHopDongVaKySo.CLIENT.Models;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMCreateFormForCus
    {
        public List<TypeOfService> TypeOfServices { get; set; }
        public API.PostPendingContract PostPendingContract { get; set; }
    }
}
