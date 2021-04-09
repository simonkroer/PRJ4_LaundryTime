using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Models.Calender;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public interface IBookingListModelRepository : IRepository<BookingListModel>
    {
        public IEnumerable<BookingListModel> GetBookingListWithDate();
    }
}
