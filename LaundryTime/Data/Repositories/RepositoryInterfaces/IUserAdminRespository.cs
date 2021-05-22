using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IUserAdminRespository
    {
        bool UserExists(string email);

        List<UserAdmin> GetAllUserAdmins();

        UserAdmin GetSingleUserAdmin(string username);
        UserAdmin GetUserAdmin(string id);
        void AddUserAdmin(UserAdmin userAdmin);

        void Update(UserAdmin userAdmin);

        void DeleteUser(UserAdmin userAdmin);
    }
}
