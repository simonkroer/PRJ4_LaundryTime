using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class ReservedBookingListModel : IdentityDbContext
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Machine { get; set; }
        public DateTime Date { get; set; }
        public string UserFirstName { get; set; }
        public User User { get; set; }
    }
}
