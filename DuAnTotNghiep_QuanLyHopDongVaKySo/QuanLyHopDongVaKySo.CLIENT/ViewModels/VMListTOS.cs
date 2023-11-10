using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo_API.Models.ViewPost;
using QuanLyHopDongVaKySo_API.Models.ViewPuts;
using API = QuanLyHopDongVaKySo_API.Models;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListTOS
    {
        public List<TypeOfService> TypeOfServices { get; set; }
        public PostTOS PostTOS { get; set; }
        public PutTOS PutTOS { get; set; }
        public List<API.TemplateMinute> TemplateMinutes { get; set; }
        public List<API.TemplateContract> TemplateContracts { get; set; }
    }
}
