using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class DoneMinute
    {
        [Key]
        [Column("Id")]
        public int DoneMinuteID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hoàn thành")]
        public DateTime DateDone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên biên bản")]
        public string MinuteName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tệp biên bản")]
        public string? MinuteFile { get; set; }

        public string? Base64File { get; set; }


        //Tạo khoá liên kết
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}
