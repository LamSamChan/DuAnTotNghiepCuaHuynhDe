using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class ContractCoordinate
    {
        [Key]
        [Column("Id")]
        public int ContractCoorID { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Hãy thêm tên trên trường !")]
        public string FieldName { get; set; }
        [Required(ErrorMessage = "Hãy thêm toạ độ X !")]
        public double X { get; set; }
        [Required(ErrorMessage = "Hãy thêm toạ độ Y !")]
        public double Y { get; set; }

        //tạo liên kết
        [Required(ErrorMessage = "Hãy thêm ID của hợp đồng mẫu !")]
        [ForeignKey("TemplateContract")]
        public int TContractID { get; set; }

        public TemplateContract? TemplateContract { get; set; }
    }
}
