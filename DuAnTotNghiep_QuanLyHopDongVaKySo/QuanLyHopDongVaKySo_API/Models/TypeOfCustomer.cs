using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class TypeOfCustomer
    {
        [Key]
        [Column("Id")]
        public int TOC_ID { get; set; }

        [Required(ErrorMessage = "Hãy điền tên loại khách hàng !")]
        [Display(Name = "Loại khách hàng")]
        public string TypeName { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        public DateTime DateAdded { get; set; }

        //tạo liên kết

    }
}
