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
        [ForeignKey("Employee")]
        public Guid EmployeeId { get; set; }

        [ForeignKey("Customer")]
        public Guid CustomerId { get; set; }

        [ForeignKey("TypeOfService")]
        public int TOS_ID { get; set; }

        [ForeignKey("DoneMinute")]
        public int DoneMinuteId { get; set; }


        public Employee Employee { get; set; }
        public Customer Customer { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public DoneMinute DoneMinute { get; set; }





    }
}
