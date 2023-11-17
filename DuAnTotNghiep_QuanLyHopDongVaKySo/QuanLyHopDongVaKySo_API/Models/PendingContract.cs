using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using QuanLyHopDongVaKySo_API.ViewModels;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PendingContract
    {
        [Key]
        [Column("Id")]
        public int PContractID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
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

        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }
        public string? Base64File { get; set; }

        //tạo liên kết
        [ForeignKey("Employee")]
        public Guid EmployeeCreatedId { get; set; }
        [AllowNull]
        public Guid? DirectorSignedId { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        [ForeignKey("TypeOfService")]
        public int TOS_ID { get; set; }

        [JsonIgnore]
        public Employee? Employee { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }
        [JsonIgnore]
        public TypeOfService? TypeOfService { get; set; }

        public object Select(Func<object, PContractViewModel> value)
        {
            throw new NotImplementedException();
        }
    }

}
