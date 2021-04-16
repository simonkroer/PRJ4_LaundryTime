using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;

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
            
        }

        public SystemAdmin GetSingleSystemAdmin()
        {
            throw new NotImplementedException();
        }

        public void AddSystemAdmin(SystemAdmin systemAdmin)
        {
            throw new NotImplementedException();
        }
    }
}
