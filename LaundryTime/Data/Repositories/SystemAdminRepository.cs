using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Data.Repositories
{
    public class SystemAdminRepository : Repository<SystemAdmin>, ISystemAdminRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public SystemAdminRepository(ApplicationDbContext context) : base(context) { }

        public List<SystemAdmin> GetAllSystemAdmins()
        {
            return context.SystemAdmins
                .Include(p => p.UserAdmins)
                .ThenInclude(d=>d.Machines)
                .Include(p => p.UserAdmins)
                .ThenInclude(d => d.Users)
                .Include(l => l.LaundryUsers)
                .ThenInclude(a=>a.LaundryHistory)
                .ToList();
        }

        public SystemAdmin GetSingleSystemAdmin(string username)
        {
            return context.SystemAdmins
                .Include(p => p.UserAdmins)
                .ThenInclude(d => d.Machines)
                .Include(p => p.UserAdmins)
                .ThenInclude(d => d.Users)
                .Include(l => l.LaundryUsers)
                .ThenInclude(a => a.LaundryHistory)
                .SingleOrDefault(p => p.UserName == username);
        }

        public void AddSystemAdmin(SystemAdmin systemAdmin)
        {
            context.SystemAdmins.Add(systemAdmin);
        }

        public bool UserExists(string email)
        {
            var exists = context.SystemAdmins.SingleOrDefault(d => d.Email == email);

            if (exists == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
