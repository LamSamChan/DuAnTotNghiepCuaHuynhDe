using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.ViewModels
{
    public class PContractViewModel
    {
        public string PContractID { get; set; }
        public string PContractName { get; set; }
        public string DateCreated { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerId { get; set; }
        public string IsDirector { get; set; }
        public string IsCustomer { get; set; }
        public string IsRefuse { get; set; }
        public string? DirectorSignedId { get; set; }
        public string EmployeeCreatedId { get; set; }
        public string? Reason { get; set; }
        public string InstallationAddress { get; set; }
        public string TOS_ID { get; set; }
        public string PContractFile { get; set; }
        public string Base64File { get; set; }
    }
}
