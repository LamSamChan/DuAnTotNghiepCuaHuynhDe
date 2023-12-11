using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class DoneContract
    {
        [Key]
        [Column("Id")]
        public int DContractID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hoàn thành")]
        public DateTime DateDone { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày kết thúc hợp đồng")]
        [AllowNull]
        public DateTime DateUnEffect { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        public string DConTractName { get; set; }

        [Display(Name = "Tệp hợp đồng")]
        public string? DContractFile { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsInEffect { get; set; }

        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        public string? Base64File { get; set; }

        //tạo liên kết
        [ForeignKey("Employee")]
        public Guid? EmployeeCreatedId { get; set; }
        public Guid? DirectorSignedId { get; set; }

        [ForeignKey("Customer")]
        public Guid? CustomerId { get; set; }

        [ForeignKey("TypeOfService")]
        public int TOS_ID { get; set; }

        [ForeignKey("DoneMinute"), AllowNull]
        public int? DoneMinuteId { get; set; }

        

        [JsonIgnore]
        public Employee? Employee { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public TypeOfService? TypeOfService { get; set; }
        [JsonIgnore]
        public DoneMinute? DoneMinute { get; set; }
    }
}
