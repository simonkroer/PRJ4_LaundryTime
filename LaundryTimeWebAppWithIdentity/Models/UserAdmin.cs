using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class UserAdmin : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }

        //public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Machine> Machines { get; set; }
    }
}
