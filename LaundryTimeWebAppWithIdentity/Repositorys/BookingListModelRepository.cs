using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTimeWebAppWithIdentity.Data;
using LaundryTimeWebAppWithIdentity.Models.Calender;
using Microsoft.EntityFrameworkCore;

namespace LaundryTimeWebAppWithIdentity.Repositorys
{
    public class BookingListModelRepository : Repository<BookingListModel>, IBookingListModelRepository
    {
        public BookingListModelRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<BookingListModel> GetBookingListWithDate()
        {
            return _context.BookingListModels.Include(blm => blm.DateModel).ToList();
        }
    }
}
