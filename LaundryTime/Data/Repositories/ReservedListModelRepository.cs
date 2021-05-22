using LaundryTime.Data.Models.Booking;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Repositories
{
    public class ReservedListModelRepository: Repository<ReservedListModel>, IReservedListRepository
    {
        public ApplicationDbContext context
        {
            get { return Context as ApplicationDbContext; }
        }
        public ReservedListModelRepository(ApplicationDbContext context) : base(context) { }

        public void AddSingleReservation(ReservedListModel modelToAdd)
        {
            context.ReservedListModels.AddAsync(modelToAdd);
        }

        public async Task<ReservedListModel> GetUnBookOrder(long? id)
        {
            return await context.ReservedListModels.Include(b => b.Machine).FirstOrDefaultAsync(r => r.Id == id);

        }

        public void RemoveBooking(ReservedListModel bookingToRemove)
        {
            context.Remove(bookingToRemove);
        }

        public async Task<List<ReservedListModel>> GetReservedBookingList()
        {
            return await context.ReservedListModels.Include(r => r.Machine).ToListAsync();
        }
    }
}
