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
                .Include(l => l.LaundryUsers)
                .ToList();
        }

        public SystemAdmin GetSingleSystemAdmin(string id)
        {
            return context.SystemAdmins
                .Include(g=>g.LaundryUsers)
                .Include(s=>s.UserAdmins)
                .SingleOrDefault(p => p.Id == id);
        }

        public void AddSystemAdmin(SystemAdmin systemAdmin)
        {
            context.SystemAdmins.Add(systemAdmin);
        }
    }
}
