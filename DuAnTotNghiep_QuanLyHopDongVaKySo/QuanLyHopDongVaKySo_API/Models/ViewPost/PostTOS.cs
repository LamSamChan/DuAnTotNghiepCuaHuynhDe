using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models.ViewPost
{
    public class PostTOS
    {
        [Required(ErrorMessage = "Hãy điền tên loại dịch vụ !")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên loại dịch vụ")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Hãy điền giá dịch vụ !")]
        [Display(Name = "Giá")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Hãy điền đơn vị thời gian của giá !")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Đơn vị thời gian")]
        public string PerTime { get; set; }
        [Required(ErrorMessage = "Hãy điền mã hợp đồng mẫu !")]
        public int TContractID { get; set; }
        [Required(ErrorMessage = "Hãy điền mã biên bản lắp đặt mẫu !")]
        public int TMinuteID { get; set; }

    }
}
