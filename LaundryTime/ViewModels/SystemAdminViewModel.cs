using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Mvc;
using static LaundryTime.Areas.Identity.Pages.Account.RegisterModel;

namespace LaundryTime.ViewModels
{
    public class SystemAdminViewModel
    {
        public SystemAdminViewModel() { }

        [BindProperty]
        public SystemAdmin CurrentSystemAdmin { get; set; }
        [BindProperty]
        public List<UserAdmin> AllUserAdmins { get; set; }

        public UserAdmin UserAdmin { get; set; }

        public List<LaundryUser> AllUsers { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
    }
}
