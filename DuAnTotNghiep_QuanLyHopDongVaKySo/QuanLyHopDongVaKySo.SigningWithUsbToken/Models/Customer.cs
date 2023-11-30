using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuanLyHopDongVaKySo.SigningWithUsbToken.Models
{
    public class Customer
    {
        
        public Guid? CustomerId { get; set; }
 
        public string? BuisinessName { get; set; }

 
        public string FullName { get; set; }

  
        public string? Position { get; set; }

  
        public DateTime DateOfBirth { get; set; }

        public int Gender { get; set; }

        public string PhoneNumber { get; set; }

 
        public string Email { get; set; }
 
        public string Identification { get; set; }
 
        public DateTime IssuedDate { get; set; }

 
        public string IssuedPlace { get; set; }

 
        public string? PowerOfAttorneyNum { get; set; }

 
        public DateTime? DatePOA { get; set; }

 
        public string? WhoPOA { get; set; }
 
        public string? BuisinessNumber { get; set; }
 
        public DateTime? BNDate { get; set; }

        public string? BNPlace { get; set; }



        public string Nationality { get; set; }


        public string? BankAccount { get; set; }


        public string? BankName { get; set; }


        public string? TaxIDNumber { get; set; }

        public string Address { get; set; }



        public string? FAX { get; set; }


        public string ChargeNoticeAddress { get; set; }


        public string BillingAddress { get; set; }

        public bool IsLocked { get; set; }


        public string? Note { get; set; }

        //tạo liên kết

        public string? SerialPFX { get; set; }

        public string typeofCustomer { get; set; }

        
    }
}
