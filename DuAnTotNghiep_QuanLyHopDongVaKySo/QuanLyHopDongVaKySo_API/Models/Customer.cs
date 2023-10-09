using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class Customer
    {
        [Key]
        [Column("Id")]
        public Guid CustomerId { get; set; }

        [Column(TypeName = "nvarchar(100)"),AllowNull]
        [Display(Name = "Tên doanh nghiệp")]
        public string BuisinessName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Hãy điền họ và tên !")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(100)"), AllowNull]
        [Display(Name = "Chức vụ")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm sinh !")]
        [Display(Name = "Năm sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Hãy chọn giới tính !"),
            Range(1, 2, ErrorMessage = "Giới tính không hợp lệ !")]
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại !")]
        [Display(Name = "Số điện thoại")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(100)")]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Hãy điền email !")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
           ErrorMessage = "Email không hợp lệ !")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hãy số chứng minh nhân dân / căn cước công dân !")]
        [Display(Name = "CMND / CCCD")]
        [MaxLength(20)]
        public string Identification { get; set; }


    }
}
