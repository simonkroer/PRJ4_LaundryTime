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

        public bool UserExists(string email)
        {
            return context.UserAdmins.Any(e => e.Email == email);
        }

        public List<UserAdmin> GetAllUserAdmins()
        {
            return context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                    .ThenInclude(l => l.LaundryHistory)
                .Include(u => u.Users)
                    .ThenInclude(l => l.Address)
                .ToList();
        }

        public UserAdmin GetSingleUserAdmin(string email)
        {
            return context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                    .ThenInclude(s=>s.Address)
                .Include(o=>o.Users)
                    .ThenInclude(l => l.LaundryHistory)
                .SingleOrDefault(i => i.UserName == email);
        }

        public Task<UserAdmin> GetSingleUserAdminAsync(string email)
        {
            return context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                .ThenInclude(s => s.Address)
                .Include(o => o.Users)
                .ThenInclude(l => l.LaundryHistory)
                .SingleOrDefaultAsync(i => i.UserName == email);
        }



        public async Task<UserAdmin> GetUserAdmin(string id)
        {

            return await context.UserAdmins
                .Include(m => m.Machines)
                .Include(a => a.WorkAddress)
                .Include(u => u.Users)
                    .ThenInclude(s => s.Address)
                .Include(o => o.Users)
                    .ThenInclude(l => l.LaundryHistory)
                .SingleOrDefaultAsync(i => i.Id == id);
        }


        public void AddUserAdmin(UserAdmin userAdmin)
        {
            context.UserAdmins.Add(userAdmin);
        }

        public void Update(UserAdmin userAdmin)
        {
            context.Update(userAdmin);
        }

        public void DeleteUser(UserAdmin userAdmin)
        {
            context.UserAdmins.Remove(userAdmin);
        }

    }
}
