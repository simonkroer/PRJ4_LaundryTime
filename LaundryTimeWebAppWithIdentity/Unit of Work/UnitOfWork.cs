using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Models;
using LaundryTimeWebAppWithIdentity.Repositorys;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting.Internal;

namespace LaundryTimeWebAppWithIdentity.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; private set; }
        public IBookingListModelRepository BookingLists { get; private set; }
        public IDateModelRepository DateModelLists { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            BookingLists = new BookingListModelRepository(_context);
            DateModelLists = new DateModelRepository(_context);
        }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
