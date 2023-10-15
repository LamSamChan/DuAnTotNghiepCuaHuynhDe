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
        public string Serial { get; set; }

        [Display(Name = "Tên tệp PFX")]
        [Column("Name")]
        public string PfxFileName { get; set; }

        [Display(Name = "Mật khẩu tệp PFX")]
        [Column("Password")]
        public string PfxPassword { get; set; }


        [Display(Name = "Tổ chức phát hành")]
        [Column("Issuer")]
        public string Issuer { get; set; }

        [Display(Name = "Người sở hữu")]
        [Column("Subject")]
        public string Subject { get; set; }

        [Display(Name = "Giá trị băm")]
        [Column("Thumbprint")]
        public string Thumbprint { get; set; }
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

        [NotMapped]
        [Display(Name = "Tệp ảnh")]
        public IFormFile? ImageFile { get; set; }

        //tạo liên kết

    }
}
