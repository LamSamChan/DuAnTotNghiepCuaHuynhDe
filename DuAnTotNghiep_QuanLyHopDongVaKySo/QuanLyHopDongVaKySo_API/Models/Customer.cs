using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public enum Gender
    {
        [Display(Name = "Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 2
    }
    public class Customer
    {
        [Key]
        [Column("Id")]
        public Guid? CustomerId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateAdded { get; set; }

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

        [Column(TypeName = "varchar(100)"),StringLength(50)]
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

        [Column(TypeName = "nvarchar(30)"), AllowNull]
        [Display(Name = "Giấy uỷ quyền")]
        [MaxLength(30)]
        public string? PowerOfAttorneyNum { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}"),AllowNull]
        [Display(Name = "Ngày")]
        public DateTime? DatePOA { get; set; }

        [Column(TypeName = "nvarchar(50)"), AllowNull]
        [Display(Name = "Của")]
        public string? WhoPOA { get; set; }

        [Column(TypeName = "nvarchar(50)"), AllowNull]
        [Display(Name = "Số chứng chỉ đăng ký doanh nghiệp")]
        public string? BuisinessNumber { get; set; }

        [AllowNull]
        [Display(Name = "Ngày cấp chứng chỉ")]
        public DateTime? BNDate { get; set; }

        [Column(TypeName = "nvarchar(50)"), AllowNull]
        [Display(Name = "Nơi cấp chứng chỉ")]
        public string? BNPlace { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Hãy nhập Quốc Tịch!")]
        [Display(Name = "Quốc tịch")]
        public string Nationality { get; set; }

        [AllowNull]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Số tài khoản ngân hàng")]
        public string? BankAccount { get; set; }

        [AllowNull]
        [Column(TypeName = "varchar(50)")]
        [Display(Name = "Tên ngân hàng")]
        public string? BankName { get; set; }

        [Column(TypeName = "varchar(50)"), AllowNull]
        [Display(Name = "Mã số thuế")]
        public string? TaxIDNumber { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required(ErrorMessage = "Hãy nhập địa chỉ thường chú !")]
        [Display(Name = "Địa chỉ thường chú ")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(50)"), AllowNull]
        [Display(Name = "FAX")]
        public string? FAX { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required(ErrorMessage = "Hãy nhập địa gửi giấy báo cước !")]
        [Display(Name = "Địa chỉ gửi giấy báo cước")]
        public string ChargeNoticeAddress { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Required(ErrorMessage = "Hãy nhập địa chỉ xuất hóa đơn!")]
        [Display(Name = "Địa chỉ xuất hóa đơn")]
        public string BillingAddress { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public bool IsLocked { get; set; }

        [Column(TypeName = "nvarchar(255)"), AllowNull]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        //tạo liên kết
        [ForeignKey("PFXCertificate")]
        public string? SerialPFX { get; set; }

        public string typeofCustomer { get; set; }

        [JsonIgnore]
        public PFXCertificate? PFXCertificate { get; set; }
        [JsonIgnore]
        public ICollection<DoneContract>? DoneContract { get; set; }
        [JsonIgnore]
        public ICollection<PendingContract>? PendingContract { get; set; }
        [JsonIgnore]
        public ICollection<OperationHistoryCus>? OperationHistoryCus { get; set; }
    }
}
