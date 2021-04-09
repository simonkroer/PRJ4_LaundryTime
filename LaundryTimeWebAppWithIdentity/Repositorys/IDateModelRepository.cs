using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Models.Calender;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public interface IDateModelRepository : IRepository<DateModel>
    {
        public IEnumerable<DateModel> GetDateWithBookingList();
    }
}
