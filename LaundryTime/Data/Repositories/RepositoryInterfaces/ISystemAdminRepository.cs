using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface ISystemAdminRepository
    {
        List<SystemAdmin> GetAllSystemAdmins();

        SystemAdmin GetSingleSystemAdmin(string id);

        void AddSystemAdmin(SystemAdmin systemAdmin);
    }
}
