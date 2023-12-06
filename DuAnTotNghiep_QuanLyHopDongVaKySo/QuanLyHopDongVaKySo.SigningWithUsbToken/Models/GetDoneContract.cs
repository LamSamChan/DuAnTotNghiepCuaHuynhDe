using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Models
{
    public class GetDoneContract
    {
        public int DContractID { get; set; }

        [DisplayFormat(DataFormatString = "{0:HH:mm:ss dd/MM/yyyy}")]
        [Display(Name = "Ngày hoàn thành")]
        public DateTime DateDone { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "Tên hợp đồng")]
        public string DConTractName { get; set; }

        [Display(Name = "Tệp hợp đồng")]
        public string? DContractFile { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsInEffect { get; set; }

        [Display(Name = "Địa chỉ lắp đặt")]
        public string InstallationAddress { get; set; }

        public string? Base64File { get; set; }

        public Guid? EmployeeCreatedId { get; set; }
        public Guid? DirectorSignedId { get; set; }

        public Guid? CustomerId { get; set; }

        public int TOS_ID { get; set; }

        public int? DoneMinuteId { get; set; }

    }
}
