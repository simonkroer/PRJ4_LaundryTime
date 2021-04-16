using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using LaundryTime.Data.Models;

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
    }
}
