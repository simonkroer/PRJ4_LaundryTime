using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.ViewModels
{
    public class SystemAdminViewModel
    {
        public SystemAdminViewModel() { }

        public SystemAdmin CurrentSystemAdmin { get; set; }

        public List<UserAdmin> AllUserAdmins { get; set; }
    }
}
