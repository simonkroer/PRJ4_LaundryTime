using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Models;
using Microsoft.Extensions.Hosting.Internal;

namespace LaundryTimeWebAppWithIdentity.Unit_of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
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
