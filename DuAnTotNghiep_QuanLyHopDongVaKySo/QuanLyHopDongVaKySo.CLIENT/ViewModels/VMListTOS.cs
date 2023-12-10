using QuanLyHopDongVaKySo.CLIENT.Models;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPost;
using QuanLyHopDongVaKySo.CLIENT.Models.ModelPut;

namespace QuanLyHopDongVaKySo.CLIENT.ViewModels
{
    public class VMListTOS
    {
        public List<TypeOfService> TypeOfServices { get; set; }
        public PostTOS PostTOS { get; set; }
        public PutTOS PutTOS { get; set; }
        public List<TemplateMinute> TemplateMinutes { get; set; }
        public List<TemplateContract> TemplateContracts { get; set; }
    }
}
