using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
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
        public string TContractFile{ get; set; }

        public int x_DirectorZone { get; set; }
        public int y_DirectorZone { get; set; }

        public int x_CustomerZone { get; set; }
        public int y_CustomerZone { get; set; }

        //tạo liên kết


    }
}
