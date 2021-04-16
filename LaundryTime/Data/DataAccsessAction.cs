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

        public DataAccsessAction(ApplicationDbContext context)
        {
            _context = context;
            Machines = new MachineRepository(_context);
        }
        public int Complete()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
