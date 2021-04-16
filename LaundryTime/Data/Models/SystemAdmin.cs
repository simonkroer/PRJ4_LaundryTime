using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LaundryTime.Data.Models
{
    public class SystemAdmin: IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey("Id")]
        public List<UserAdmin> UserAdmins { get; set; }

        [ForeignKey("Id")]
        public List<LaundryUser> LaundryUsers { get; set; }
    }
}
