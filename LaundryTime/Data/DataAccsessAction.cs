using LaundryTime.Data.Repositories;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data
{
    public class DataAccsessAction : IDataAccessAction
    {
        private readonly ApplicationDbContext _context;
        public IMachineRepository Machines { get; private set; }

        public IAddressRepository Addresses { get; private set; }

        public ILaundryUserRepository LaundryUsers { get; private set; }

        public IUserAdminRespository UserAdmins { get; private set; }

        public ISystemAdminRepository SystemAdmins { get; private set; }

        public IBookingListRepository BookingList { get; private set; }

        public ILaundryLogRepository LaundryLogs { get; private set; }

        public IReservedListRepository ReservedList { get; private set; }
        public IMessageToUserAdminRepository MessageList { get; private set; }
        public DataAccsessAction(ApplicationDbContext context)
        {
            _context = context;
            Machines = new MachineRepository(_context);
            Addresses = new AddressRepository(_context);
            LaundryUsers = new LaundryUserRepository(_context);
            UserAdmins = new UserAdminRepository(_context);
            SystemAdmins = new SystemAdminRepository(_context);
            BookingList = new BookingListRepository(_context);
            LaundryLogs = new LaundryLogRepository(_context);
            ReservedList = new ReservedListModelRepository(_context);
            MessageList = new MessageToUserAdminRepository(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }
        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
