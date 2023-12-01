using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Models
{
    public class TemplateContract
    {
        [Key]
        [Column("Id")]
        public int TContactID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày thêm")]
        public DateTime DateAdded { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        [Required(ErrorMessage = "Hãy điền tên mẫu hợp đồng !")]
        public string TContractName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tệp hợp đồng")]
        public string? TContractFile { get; set; }

        [AllowNull]
        [Column(TypeName = "nvarchar(50)"), MaxLength(50)]
        public string? jsonDirectorZone { get; set; }
        [AllowNull]
        [Column(TypeName = "nvarchar(50)"),MaxLength(50)]
        public string? jsonCustomerZone { get; set; }

        public string? Base64File { get; set; }


    }
}
