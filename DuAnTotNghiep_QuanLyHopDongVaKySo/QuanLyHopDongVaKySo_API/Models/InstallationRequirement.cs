using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace QuanLyHopDongVaKySo_API.Models
{
    //Yêu cầu lắp đặt
    public class InstallationRequirement
    {
        [Key]
        [Column("Id")]
        public int InstallRequireID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên biên bản")]
        public string MinuteName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        //tạo liên kết
        [ForeignKey("DoneContract")]
        public int DoneContractId { get; set; }

        public DoneContract? DoneContract { get; set; }


    }
}
