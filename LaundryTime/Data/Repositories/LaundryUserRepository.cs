using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;

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
            throw new NotImplementedException();
        }

        public LaundryUser GetSingleLaundryUser()
        {
            throw new NotImplementedException();
        }

        public void AddLaundryUser(LaundryUser laundryUser)
        {
            throw new NotImplementedException();
        }
    }
}
