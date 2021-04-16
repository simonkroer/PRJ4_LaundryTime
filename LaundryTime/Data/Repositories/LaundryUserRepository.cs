using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Data.Repositories
{
    public class LaundryUserRepository : Repository<LaundryUser>, ILaundryUserRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public LaundryUserRepository(ApplicationDbContext context) : base(context) { }

        public List<LaundryUser> GetAllLaundryUsers()
        {
            return context.LaundryUsers.ToList();
        }

        public LaundryUser GetSingleLaundryUser(string username)
        {
            return context.LaundryUsers
                .Include(p => p.Administrator)
                .Include(t => t.LaundryHistory)
                .SingleOrDefault(i => i.UserName == username);
        }

        public void AddLaundryUser(LaundryUser laundryUser)
        {
            context.LaundryUsers.Add(laundryUser);
        }

        public bool LaudryUserExists(string email)
        {
            return context.ApplicationUsers.Any(e => e.Email == email);
        }
    }
}
