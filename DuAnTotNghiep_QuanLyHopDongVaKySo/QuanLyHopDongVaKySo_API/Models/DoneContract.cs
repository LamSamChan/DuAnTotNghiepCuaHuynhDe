using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class DoneContract
    {
        [Key]
        [Column("Id")]
        public int DContractID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hoàn thành")]
        public DateTime DateDone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        public string DConTractName { get; set; }

        [Display(Name = "Tệp hợp đồng")]
        public string DContractFile { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsInEffect { get; set; }

        //tạo liên kết

    }
}
