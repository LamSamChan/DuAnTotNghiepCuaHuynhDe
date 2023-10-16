

using System.ComponentModel.DataAnnotations;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PostCustomer
    {
        public string BuisinessName { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Position { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Identification { get; set; }
        [Required]
        public DateTime IssuedDate { get; set; }
        [Required]
        public string IssuedPlace { get; set; }
         [Required]
        public string Nationality { get; set; }
        [Required]
        public string BankAccount { get; set; }
        [Required]
        public string BankName { get; set; }
        public string TaxIDNumber { get; set; }
        [Required]
        public string BillingAddress { get; set; }
        public string? Note { get; set; }
        public int TOC_ID { get; set; }
    }
}