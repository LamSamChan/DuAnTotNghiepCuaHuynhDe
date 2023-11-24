using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutPendingContract
    {
        public int PContractId { get; set; }
        public DateTime DateCreated { get; set; }

        public string? PContractName { get; set; }

        public string? PContractFile { get; set; }

        public bool IsDirector { get; set; }

        public bool IsCustomer { get; set; }

        public bool IsRefuse { get; set; }
        public string? InstallationAddress { get; set; }

        public string? Reason { get; set; }

        public Guid? EmployeeCreatedId { get; set; }
        public Guid? DirectorSignedId { get; set; }

        public Guid? CustomerId { get; set; }

        public int TOS_ID { get; set; }

        public string? Base64File { get; set; }
    }

}
