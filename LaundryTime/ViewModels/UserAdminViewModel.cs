using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.ViewModels
{
    public class UserAdminViewModel
    {
        public List<LaundryUser> MyUsers { get; set; }

        public List<Machine> MyMachines { get; set; }

        public Machine CurrentMachine { get; set; }

        public LaundryUser CurrentLaundryUser { get; set; }
    }
}
