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

        UserAdmin GetSingleUserAdmin();

        void AddUserAdmin(UserAdmin userAdmin);
    }
}
