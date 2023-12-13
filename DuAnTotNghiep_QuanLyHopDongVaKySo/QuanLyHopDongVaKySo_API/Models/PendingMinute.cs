using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PendingMinute
    {
        [Key]
        [Column("Id")]
        public int PendingMinuteId { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên biên bản")]
        public string MinuteName { get; set; }

        [Display(Name = "Nhân viên lắp đặt đã ký")]
        public bool IsIntallation { get; set; }

        [Display(Name = "Khách hàng đã ký")]
        public bool IsCustomer { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tệp biên bản")]
        public string MinuteFile { get; set; }
        public string? Base64File { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        //tạo liên kết
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        [ForeignKey("DoneContract")]
        public int DoneContractId { get; set; }


        [JsonIgnore]
        public Employee? Employee { get; set; }
        [JsonIgnore]
        public DoneContract? DoneContract { get; set; }
    }
}
