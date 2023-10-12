using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class OperationHistory
    {
        [Key]
        [Column("Id")]
        public double HistoryID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày")]
        public DateTime Date { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        [Display(Name = "Tên thao tác")]
        public string OperationName { get; set; }
    }
}
