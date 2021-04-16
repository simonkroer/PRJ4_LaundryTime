using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IUserAdminRespository
    {
        List<UserAdmin> GetAllUserAdmins();

        UserAdmin GetSingleUserAdmin(string id);

        void AddUserAdmin(UserAdmin userAdmin);
    }
}
