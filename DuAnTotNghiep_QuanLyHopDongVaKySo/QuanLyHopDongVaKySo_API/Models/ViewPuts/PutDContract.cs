
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutDContract
    {
        [Column("Id")]
        public string DContractID { get; set; }

        [Display(Name = "Ngày hoàn thành")]
        public DateTime DateDone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        public string DContractName { get; set; }

        [Display(Name = "Tệp hợp đồng")]
        public string DContractFile { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsInEffect { get; set; }

        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        //tạo liên kết
        public Guid? EmployeeCreatedId { get; set; }
        public Guid? DirectorSignedId { get; set; }

        public Guid? CustomerId { get; set; }

        public int TOS_ID { get; set; }

        public int? DoneMinuteId { get; set; }

        public string? Base64File { get; set; }
    }
}