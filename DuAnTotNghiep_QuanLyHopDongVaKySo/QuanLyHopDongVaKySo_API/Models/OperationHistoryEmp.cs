using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class OperationHistoryEmp
    {
        [Key]
        [Column("Id")]
        public int HistoryID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày")]
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Tên thao tác")]
        public string OperationName { get; set; }

        //tạo liên kết
        [ForeignKey("Employee")]
        public Guid? EmployeeID { get; set; }

        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}
