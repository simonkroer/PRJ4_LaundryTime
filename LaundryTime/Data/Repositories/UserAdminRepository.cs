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
                .Include(u => u.Users)
                .ToList();
        }

        public UserAdmin GetSingleUserAdmin(string id)
        {
            return context.UserAdmins.SingleOrDefault(u => u.Id == id);
        }

        public void AddUserAdmin(UserAdmin userAdmin)
        {
            context.UserAdmins.Add(userAdmin);
        }
    }
}
