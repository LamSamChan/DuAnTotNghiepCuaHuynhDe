using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostPendingContract
    {

        public string PContractName { get; set; }

        public string PContractFile { get; set; }

        public bool IsRefuse { get; set; }

        public string? Reason { get; set; }

        public Guid EmployeeId { get; set; }
        public Guid CustomerId { get; set; }

        public int TOS_ID { get; set; }

        public int TContractId { get; set; }

    }

}
