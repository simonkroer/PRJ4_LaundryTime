using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTimeWebAppWithIdentity.Unit_of_Work
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();

    }
}
