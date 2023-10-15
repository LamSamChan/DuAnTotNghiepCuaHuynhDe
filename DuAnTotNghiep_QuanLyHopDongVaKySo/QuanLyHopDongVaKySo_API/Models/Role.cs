using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace QuanLyHopDongVaKySo_API.Models
{
    public class Role
    {
        [Key]
        [Column("Id")]
        public int RoleID { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Hãy điền tên vai trò !")]
        public string RoleName { get; set; }
        public bool isHidden { get; set; }

        //tạo liên kết

    }
}
