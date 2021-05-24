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

        public async Task<SystemAdmin> GetSingleSystemAdminAsync(string email)
        {
            return await context.SystemAdmins
                .Include(p => p.UserAdmins)
                    .ThenInclude(d => d.Machines)
                .Include(p => p.UserAdmins)
                    .ThenInclude(d => d.Users)
                .Include(l => l.LaundryUsers)
                    .ThenInclude(a => a.LaundryHistory)
                .SingleOrDefaultAsync(p => p.UserName == email);
        }

        public void AddSystemAdmin(SystemAdmin systemAdmin)
        {
            context.SystemAdmins.Add(systemAdmin);
        }

        public bool UserExists(string email)
        {
            return context.SystemAdmins.Any(d => d.Email == email);
            
        }

        public async Task<UserAdmin> GetCurrentUserAdmin(string email)
        {
            var user = await context.SystemAdmins
                .SingleOrDefaultAsync(p => p.Email == email);

            return await context.UserAdmins
                    .Include(m => m.Machines)
                    .Include(a => a.WorkAddress)
                    .Include(u => u.Users)
                    .ThenInclude(s => s.Address)
                    .Include(o => o.Users)
                    .ThenInclude(l => l.LaundryHistory)
                    .SingleOrDefaultAsync(i => i.UserName == user.CurrentUserAdminName);
        }

        public void Update(SystemAdmin systemAdmin)
        {
            context.Update(systemAdmin);
        }
    }
}
