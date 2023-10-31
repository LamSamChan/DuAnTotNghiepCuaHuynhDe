using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostPendingContract
    {

        //public string PContractName { get; set; }

        //public bool IsRefuse { get; set; }

        public string? Reason { get; set; }

        public Guid EmployeeCreatedId { get; set; }
        public Guid CustomerId { get; set; }
        public string InstallationAddress { get; set; }
        public int TOS_ID { get; set; }

        public int TContractId { get; set; }
        

    }

}
