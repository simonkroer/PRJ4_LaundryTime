using LaundryTime.Data.Models;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories
{
    public class LaundryLogRepository : Repository<LaundryLog>, ILaundryLogRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public LaundryLogRepository(ApplicationDbContext context) : base(context) { }

        public bool LaundryLogExists(string info)
        {
            return context.LaundryLogs.Any(e => e.LogInfo == info);
        }

        public void AddLaundryLog(LaundryLog logEntry)
        {
            context.LaundryLogs.Add(logEntry);
        }
    }
}
