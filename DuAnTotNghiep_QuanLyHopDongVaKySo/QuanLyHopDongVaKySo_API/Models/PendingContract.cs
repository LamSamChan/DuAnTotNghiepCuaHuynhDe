using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PendingContract
    {
        [Key]
        [Column("Id")]
        public int PContractID { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        public string PContractName { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Display(Name = "Tệp hợp đồng")]
        public string PContractFile { get; set; }

        [Display(Name = "Giám đốc đã ký")]
        public bool IsDirector { get; set; }

        [Display(Name = "Khách hàng đã ký")]
        public bool IsCustomer { get; set; }

        [Display(Name = "Giám đốc đã từ chối ký")]
        public bool IsRefuse { get; set; }

        [Display(Name = "Lý do"),AllowNull]
        public string? Reason { get; set; }
    }

}
