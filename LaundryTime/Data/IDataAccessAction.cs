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

        int Complete();
    }
}
