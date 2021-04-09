using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Models.Calender;
using Microsoft.EntityFrameworkCore;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public class DateModelRepository : Repository<DateModel>, IDateModelRepository
    {

        public DateModelRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<DateModel> GetDateWithBookingList()
        {
            return _context.DateModels.Include(dm => dm.BookingListModels).ToList();
        }
    }
}
