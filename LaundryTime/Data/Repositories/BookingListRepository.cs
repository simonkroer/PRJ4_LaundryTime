using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models.Booking;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using LaundryTime.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LaundryTime.Data.Repositories
{
    public class BookingListRepository : Repository<BookingListModel>, IBookingListRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public BookingListRepository(ApplicationDbContext context) : base(context) { }


        public async Task<BookingListModel> SingleBook(long? id)
        {
            return await context.BookingListModels.Include(b => b.Machine).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<BookingListModel>> GetAllAvalableBookings(DateTime date)
        {
            return await context.BookingListModels.Where(b => b.Date.Date == date.Date).Include(b => b.Machine).ToListAsync();
        }

        public bool BookingListExsits()
        {
            return context.BookingListModels.Any();
        }
    }
}
