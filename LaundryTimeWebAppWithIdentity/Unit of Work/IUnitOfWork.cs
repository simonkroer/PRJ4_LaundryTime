using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Repositorys;

namespace LaundryTimeWebAppWithIdentity.Unit_of_Work
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IBookingListModelRepository BookingLists { get; }
        IDateModelRepository DateModelLists { get; }
        int Complete();

    }
}
