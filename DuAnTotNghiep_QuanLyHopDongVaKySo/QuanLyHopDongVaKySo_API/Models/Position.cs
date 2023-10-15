using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace QuanLyHopDongVaKySo_API.Models
{
    public class Position
    {
        [Key]
        [Column("Id")]
        public int PositionID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Chức vụ")]
        [Required(ErrorMessage = "Hãy điền tên chức vụ !")]
        public string PositionName { get; set; }
        public bool isHidden { get; set; }
        //tạo liên kết

    }
}
