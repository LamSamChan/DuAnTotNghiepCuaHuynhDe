using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class InstallationMinute
    {
        [Key]
        [Column("Id")]
        public int InstallationMinuteID { get; set; }

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
    }
}
