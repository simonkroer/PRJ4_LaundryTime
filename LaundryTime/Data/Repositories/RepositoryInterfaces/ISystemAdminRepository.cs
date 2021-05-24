using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface ISystemAdminRepository
    {
        public List<SystemAdmin> GetAllSystemAdmins();

        public SystemAdmin GetSingleSystemAdmin(string username);
        Task<SystemAdmin> GetSingleSystemAdminAsync(string username);
        public void AddSystemAdmin(SystemAdmin systemAdmin);

        public bool UserExists(string email);
        Task<UserAdmin> GetCurrentUserAdmin(string email);
        public void Update(SystemAdmin systemAdmin);
    }
}
