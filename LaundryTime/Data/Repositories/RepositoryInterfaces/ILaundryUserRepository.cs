using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface ILaundryUserRepository
    {
        List<LaundryUser> GetAllLaundryUsers();

        LaundryUser GetSingleLaundryUser();

        void AddLaundryUser(LaundryUser laundryUser);
    }
}
