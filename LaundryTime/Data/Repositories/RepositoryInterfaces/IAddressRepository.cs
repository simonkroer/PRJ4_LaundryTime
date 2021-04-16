using LaundryTime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IAddressRepository: IRepository<Address>
    {
        List<Address> GetAllAdresses();
        Address GetAdress(int id);
        //void AddAdre
    }
}
