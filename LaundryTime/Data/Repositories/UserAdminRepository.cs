using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;

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
            return context.UserAdmins.ToList();
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
