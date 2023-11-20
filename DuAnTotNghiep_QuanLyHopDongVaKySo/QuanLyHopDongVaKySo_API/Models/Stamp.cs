using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class Stamp
    {
        [Key]
        [Column("Id")]
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày thêm")]
        public DateTime DateUpdated { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tên hợp đồng")]
        public string? StampPath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
