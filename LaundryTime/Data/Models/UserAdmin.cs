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
        public string Name { get; set; }

        public string PaymentMethod { get; set; }

        [ForeignKey("AddressId")]
        public Address WorkAddress { get; set; }

        [ForeignKey("Id")]
        public List<LaundryUser> Users { get; set; }

        [ForeignKey("MachineId")]
        public List<Machine> Machines { get; set; }

        public decimal FinancialBalance { get; set; }

        public DateTime PaymentDueDate { get; set; }

    }
}
