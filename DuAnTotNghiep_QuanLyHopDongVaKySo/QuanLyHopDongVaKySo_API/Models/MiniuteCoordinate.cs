using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class MinuteCoordinate
    {

        [Key]
        [Column("Id")]
        public int MinuteCoorID { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Hãy thêm tên trên trường !")]
        public string FieldName { get; set; }
        [Required(ErrorMessage = "Hãy thêm toạ độ X !")]
        public float X { get; set; }
        [Required(ErrorMessage = "Hãy thêm toạ độ Y !")]
        public float Y { get; set; }
        [Required]
        public int SignaturePage { get; set; }

        //tạo liên kết
        [Required(ErrorMessage = "Hãy thêm ID của biên bản mẫu !")]
        [ForeignKey("TemplateMinute")]
        public int TMinuteID { get; set; }

        public TemplateMinute? TemplateMinute { get; set; }
    }
}
