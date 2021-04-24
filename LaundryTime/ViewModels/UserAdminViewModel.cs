using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace LaundryTime.ViewModels
{
    public class UserAdminViewModel
    {
        public UserAdminViewModel() { }
        public List<LaundryUser> MyUsers { get; set; }

        public List<Machine> MyMachines { get; set; }

        public Machine CurrentMachine { get; set; }

        [BindProperty]
        public LaundryUser CurrentLaundryUser { get; set; }
    }
}
