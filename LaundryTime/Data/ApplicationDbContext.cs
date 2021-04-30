using LaundryTime.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LaundryTime.Data.Models.Booking;

namespace LaundryTime.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserAdmin> UserAdmins { get; set; }
        public DbSet<LaundryUser> LaundryUsers { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BookingListModel> BookingListModels { get; set; }
        public DbSet<DateModel> DateModels { get; set; }
        public DbSet<ReservedListModel> ReservedListModels { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        
    }
}
