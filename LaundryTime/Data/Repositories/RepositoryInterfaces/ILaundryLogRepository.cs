using LaundryTime.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface ILaundryLogRepository: IRepository<LaundryLog>
    {
        bool LaundryLogExists(string info);

        void AddLaundryLog(LaundryLog logEntry);
    }
}
