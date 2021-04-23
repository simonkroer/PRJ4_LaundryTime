using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models.Booking;
using LaundryTime.Data.Repositories.RepositoryInterfaces;

namespace LaundryTime.Data.Repositories
{
    public class BookingListRepository : Repository<BookingListModel>, IBookingListRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public BookingListRepository(ApplicationDbContext context) : base(context) { }

        
    }
}
