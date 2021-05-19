using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models.Booking;
using LaundryTime.ViewModels;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IBookingListRepository : IRepository<BookingListModel>
    {
        Task<BookingListModel> SingleBook(long? id);
        Task<List<BookingListModel>> GetAllAvalableBookings(DateTime date);
        bool BookingListExsits();
    }
}
