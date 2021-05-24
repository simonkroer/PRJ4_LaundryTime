using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LaundryTime.Data.Models
{
    public class UserAdmin: ApplicationUser
    {
        [Required]

        [Display(Name = "Full Name")]
        public string Name { get; set; }


        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Work Address")]
        [ForeignKey("AddressId")]
        public Address WorkAddress { get; set; }

        [ForeignKey("Id")]
        public List<LaundryUser> Users { get; set; }

        public List<Machine> Machines { get; set; }

        [Display(Name = "Financial Balance")]
        public decimal FinancialBalance { get; set; }

        [Display(Name = "Payment Due Date")]
        public DateTime PaymentDueDate { get; set; }
    }
}
