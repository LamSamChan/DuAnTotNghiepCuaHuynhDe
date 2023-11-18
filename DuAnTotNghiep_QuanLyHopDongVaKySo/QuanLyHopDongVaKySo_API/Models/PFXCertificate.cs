using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PFXCertificate
    {
        [Key]
        [Column("Id")]
        [Display(Name = "Số Seri")]
        public string? Serial { get; set; }

        [Display(Name = "Tên tệp PFX")]
        [Column("Name")]
        public string? PfxFileName { get; set; }

        [Display(Name = "Mật khẩu tệp PFX")]
        [Column("Password")]
        public string? PfxPassword { get; set; }

        [Display(Name = "Đường dẫn tệp PFX")]
        [Column("FilePath")]
        public string? PfxFilePath { get; set; }

        [Display(Name = "Tổ chức phát hành")]
        [Column("Issuer")]
        public string? Issuer { get; set; }

        [Display(Name = "Người sở hữu")]
        [Column("Subject")]
        public string? Subject { get; set; }

        [DisplayFormat(DataFormatString = "{0::HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hiệu lực")]
        [Column("ValidFrom")]
        public DateTime ValidFrom { get; set; }

        [DisplayFormat(DataFormatString = "{0::HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hết hạn")]
        [Column("ValidUntil")]
        public DateTime ValidUntil { get; set; }

        public bool IsEmployee { get; set; }

        [StringLength(500),AllowNull]
        [Display(Name = "Ảnh chữ ký 1")]
        public string? ImageSignature1 { get; set; }

        [StringLength(500), AllowNull]
        [Display(Name = "Ảnh chữ ký 2")]
        public string? ImageSignature2 { get; set; }

        [StringLength(500), AllowNull]
        [Display(Name = "Ảnh chữ ký 3")]
        public string? ImageSignature3{ get; set; }

        [StringLength(500), AllowNull]
        [Display(Name = "Ảnh chữ ký 4")]
        public string? ImageSignature4 { get; set; }

        [StringLength(500), AllowNull]
        [Display(Name = "Ảnh chữ ký 5")]
        public string? ImageSignature5 { get; set; }

        [StringLength(500), AllowNull]
        [Display(Name = "Ảnh chữ ký mặc định")]
        public string? DefaultImageSignature { get; set; }

        //tạo liên kết

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        public string? Base64StringFile { get; set; }
    }
}
