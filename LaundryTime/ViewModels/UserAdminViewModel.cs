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
        [BindProperty]
        public UserAdmin CurrentUserAdmin { get; set; }

        public string CurrentUserAdminUserName { get; set; }

        [BindProperty]
        public List<LaundryUser> MyUsers { get; set; }

        [BindProperty]
        public List<Machine> MyMachines { get; set; }

        [BindProperty]
        public Machine CurrentMachine { get; set; }

        [BindProperty]
        public LaundryUser CurrentLaundryUser { get; set; }
        public bool SortDate { get; set; }
        [BindProperty]
        public List<MessageToUserAdmin> MyMessages { get; set; }
        
    }
}
