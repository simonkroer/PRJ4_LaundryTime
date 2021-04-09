using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class User : IdentityDbContext
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ReservedBookingListModel> reservedBooking { get; set; }
    }
}
