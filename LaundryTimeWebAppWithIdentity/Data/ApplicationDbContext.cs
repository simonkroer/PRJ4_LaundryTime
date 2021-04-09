using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LaundryTimeWebAppWithIdentity.Models;

namespace LaundryTimeWebAppWithIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder ob)
        {
            ob.UseSqlServer(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LandryTimeUser;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ReservedBookingListModel> ReservedBookingListModels { get; set; }
        public DbSet<BookingListModel> BookingListModels { get; set; }
        public DbSet<DateModel> DateModels { get; set; }    
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<User>()
                .HasKey(u => new {u.FirstName, u.LastName});
            mb.Entity<User>()
                .HasMany<ReservedBookingListModel>(u => u.reservedBooking)
                .WithOne(rblm => rblm.User)
                .HasForeignKey(u => u.Id);

            mb.Entity<ReservedBookingListModel>().HasKey(rblm=>rblm.Id);

            mb.Entity<DateModel>().HasKey(dm => dm.Id);
            mb.Entity<DateModel>()
                .HasMany<BookingListModel>(dm => dm.BookingListModels)
                .WithOne(blm => blm.DateModel)
                .HasForeignKey(dm => dm.Id);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
    }
}
