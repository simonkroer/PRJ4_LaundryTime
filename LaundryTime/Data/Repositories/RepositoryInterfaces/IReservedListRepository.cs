using LaundryTime.Data.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories.RepositoryInterfaces
{
    public interface IReservedListRepository: IRepository<ReservedListModel>
    {
        void AddSingleReservation(ReservedListModel modelToAdd);

        Task<ReservedListModel> GetUnBookOrder(long? id);
        void RemoveBooking(ReservedListModel bookingToRemove);
        Task<List<ReservedListModel>> GetReservedBookingList();
    }
}
