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

        LaundryUser GetSingleLaundryUser(string username);

        public LaundryUser GetSingleLaundryUserById(string id);

        void AddLaundryUser(LaundryUser laundryUser);

        bool LaundryUserExists(string email);

        void Update(LaundryUser laundryUser);

        void DeleteUser(LaundryUser email);
    }
}
