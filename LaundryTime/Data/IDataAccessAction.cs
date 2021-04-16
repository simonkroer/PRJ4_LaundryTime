using LaundryTime.Data.Repositories.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data
{
    public interface IDataAccessAction : IDisposable
    {
        IMachineRepository Machines { get; }
        IAddressRepository Address { get; }
        public ILaundryUserRepository LaundryUsers { get; }

        public IUserAdminRespository UserAdmins { get; }

        public ISystemAdminRepository SystemAdmin { get; }
        IAddressRepository Addresses { get; }

        int Complete();
    }
}
