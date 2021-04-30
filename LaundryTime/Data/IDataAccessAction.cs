using LaundryTime.Data.Repositories.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data
{
    public interface IDataAccessAction : IDisposable
    {
        public IMachineRepository Machines { get; }
        public ILaundryUserRepository LaundryUsers { get; }

        public IUserAdminRespository UserAdmins { get; }

        public ISystemAdminRepository SystemAdmins { get; }
        public IAddressRepository Addresses { get; }
        public ILaundryLogRepository LaundryLogs { get; }
        public IBookingListRepository BookingList { get; }

        int Complete();
    }
}
