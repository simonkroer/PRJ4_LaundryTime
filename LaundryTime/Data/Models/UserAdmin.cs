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
    public class UserAdmin: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string PaymentMethod { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        private List<IdentityUser> Users { get; set; }

        
        private List<Machine> Machines { get; set; }



    }
}
