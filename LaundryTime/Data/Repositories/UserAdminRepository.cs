using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Data.Repositories
{
    public class UserAdminRepository : Repository<UserAdmin>, IUserAdminRespository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public UserAdminRepository(ApplicationDbContext context) : base(context) { }

        public List<UserAdmin> GetAllUserAdmins()
        {
            return context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                    .ThenInclude(l => l.LaundryHistory)
                .ToList();
        }

        public UserAdmin GetSingleUserAdmin(string username)
        {
            return context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                    .ThenInclude(l => l.LaundryHistory)
                .SingleOrDefault(i => i.UserName == username);
        }

        public void AddUserAdmin(UserAdmin userAdmin)
        {
            context.UserAdmins.Add(userAdmin);
        }
    }
}
