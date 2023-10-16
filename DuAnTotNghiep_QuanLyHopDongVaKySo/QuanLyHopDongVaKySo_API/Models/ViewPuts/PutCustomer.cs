using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuanLyHopDongVaKySo_API.Models
{
    public class PutCustomer
    {
        public Guid CustomerId { get; set; }
        public string BuisinessName { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Identification { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedPlace { get; set; }
        public string Nationality { get; set; }
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string TaxIDNumber { get; set; }
        public string BillingAddress { get; set; }
        public bool IsLocked { get; set; }
        public string? Note { get; set; }
        public string SerialPFX { get; set; }
        public int TOC_ID { get; set; }

    }
}
