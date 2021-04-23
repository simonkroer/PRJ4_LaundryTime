using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LaundryTimeWebAppWithIdentity.Models;
using LaundryTimeWebAppWithIdentity.Models.Calender;

namespace LaundryTimeWebAppWithIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LaundryTimeUser;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
            mb.Entity<ReservedBookingListModel>().HasKey(rblm=>rblm.Id);

        //    //mb.Entity<DateModel>().HasKey(dm => dm.Id);
        //    //mb.Entity<DateModel>()
        //    //    .HasMany<BookingListModel>(dm => dm.BookingListModels)
        //    //    .WithOne(blm => blm.DateModel)
        //    //    .HasForeignKey(dm => dm.Id);
        //}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<UserAdmin> _UserAdmins { get; set; }
        public DbSet<Machine> _Machines { get; set; }
    }
}
    