using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutCustomer
    {
        [Required(ErrorMessage = "Hãy điền ID khách hàng!")]
        public Guid CustomerId { get; set; }

        [Column(TypeName = "nvarchar(100)"), AllowNull]
        [Display(Name = "Tên doanh nghiệp")]
        public string? BuisinessName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Hãy điền họ và tên !")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(100)"), AllowNull]
        [Display(Name = "Chức vụ")]
        public string? Position { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm sinh !")]
        [Display(Name = "Năm sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Hãy chọn giới tính !"),
            Range(1, 2, ErrorMessage = "Giới tính không hợp lệ !")]
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại !")]
        [RegularExpression("^(?:\\+84|0)\\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ !")]
        [Display(Name = "Số điện thoại")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(100)"), StringLength(50)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Hãy điền email !")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
           ErrorMessage = "Email không hợp lệ !")]
        public string Email { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Hãy số chứng minh nhân dân / căn cước công dân !")]
        [Display(Name = "CMND/CCCD")]
        [MaxLength(20)]
        public string Identification { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm cấp CMND/CCCD!")]
        [Display(Name = "Ngày cấp")]
        public DateTime IssuedDate { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Hãy nhập nơi cấp CMND/CCCD!")]
        [Display(Name = "Nơi cấp")]
        public string IssuedPlace { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Hãy nhập Quốc Tịch!")]
        [Display(Name = "Quốc tịch")]
        public string Nationality { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Hãy nhập số tài khoản ngân hàng!")]
        [Display(Name = "Số tài khoản ngân hàng")]
        public string BankAccount { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Hãy nhập tên ngân hàng!")]
        [Display(Name = "Tên ngân hàng")]
        public string BankName { get; set; }

        [Column(TypeName = "varchar(50)"), AllowNull]
        [Display(Name = "Mã số thuế")]
        public string? TaxIDNumber { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required(ErrorMessage = "Hãy nhập địa chỉ thường chú / giao dịch!")]
        [Display(Name = "Địa chỉ thường chú / giao dịch")]
        public string Address { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public bool IsLocked { get; set; }

        [Column(TypeName = "nvarchar(255)"), AllowNull]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        //tạo liên kết
        [Required(ErrorMessage = "Hãy ID loại khách hàng")]
        public int TOC_ID { get; set; }

    }
}
