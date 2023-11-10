using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class InstallationDevice
    {
        [Key]
        [Column("Id")]
        public int Device_ID { get; set; }

        [Required(ErrorMessage = "Hãy điền tên thiết bị !")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên thiết bị")]
        public string DeviceName { get; set; }

        [Required(ErrorMessage = "Hãy điền trạng thái thiết bị !")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Trạng thái thiết bị")]
        public string DeviceStatus { get; set; }

        [Required(ErrorMessage = "Hãy điền số lượng thiết bị !")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Số lượng thiết bị")]
        public int DeviceQuantity { get; set; }

        //tạo liên kết
        [ForeignKey("TypeOfService")]
        [Required(ErrorMessage = "Hãy điền mã loại dịch vụ !")]
        [Display(Name = "Loại dịch vụ")]
        public int? TOS_ID { get; set; }

        [JsonIgnore]
        public TypeOfService? TypeOfService { get; set; }

    }
}
