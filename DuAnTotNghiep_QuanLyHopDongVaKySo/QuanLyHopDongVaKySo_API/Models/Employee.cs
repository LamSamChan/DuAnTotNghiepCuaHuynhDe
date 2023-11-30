using NSwag.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class Employee
    {
        [Key]
        [Column("Id")]
        public Guid? EmployeeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Hãy điền họ và tên !")]
        public string FullName { get; set; }

        [Column(TypeName = "varchar(100)"),StringLength(50)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Hãy điền email !")]
        [RegularExpression("^[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*@[a-zA-Z0-9]+(?:\\.[a-zA-Z0-9]+)*$",
            ErrorMessage = "Email không hợp lệ !")]
        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Hãy chọn ngày,tháng,năm sinh !")]
        [Display(Name = "Năm sinh")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Hãy chọn giới tính !"),
            Range(1, 2, ErrorMessage = "Giới tính không hợp lệ !")]
        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Hãy nhập số điện thoại !")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression("^(?:\\+84|0)\\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ !")]
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "varchar(50)")]
        [Required(ErrorMessage = "Hãy số chứng minh nhân dân / căn cước công dân !")]
        [Display(Name = "CMND / CCCD")]
        [MaxLength(20)]
        public string Identification { get; set; }


        [Column(TypeName = "nvarchar(255)")]
        [Required(ErrorMessage = "Hãy nhập địa chỉ !")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(500),AllowNull]
        [Display(Name = "Ảnh đại diện")]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        //tự động tạo và gửi qua mail ghi đăng ký

        [Column(TypeName = "varchar(50)"), MaxLength(50), MinLength(8, ErrorMessage = "Mật khẩu phải nhiều hơn 8 ký tự !")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [Display(Name = "Trạng thái hoạt động")]
        public bool IsLocked { get; set; }

        [Column(TypeName = "nvarchar(255)"), AllowNull]
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }

        public bool IsFirstLogin { get; set; }

        //tạo liên kết
        [ForeignKey("PFXCertificate")]
        public string? SerialPFX { get; set; }

        [Required(ErrorMessage = "Hãy nhập id của vai trò !")]
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        [Required(ErrorMessage = "Hãy nhập id của chức vụ !")]
        [ForeignKey("Position")]
        public int PositionID { get; set; }

        [JsonIgnore]
        public PFXCertificate? PFXCertificate { get; set; }
        [JsonIgnore]
        public Role? Role { get; set; }
        [JsonIgnore]
        public Position? Position { get; set; }
        [JsonIgnore]
        public ICollection<DoneContract>? DoneContract { get; set; }
        [JsonIgnore]
        public ICollection<DoneMinute>? DoneMinute { get; set; }
        [JsonIgnore]
        public ICollection<PendingContract>? PendingContract { get; set; }
        [JsonIgnore]
        public ICollection<PendingMinute>? PendingMinute { get; set; }
        [JsonIgnore]
        public ICollection<OperationHistoryEmp>? OperationHistoryEmp { get; set; }

    }
}
