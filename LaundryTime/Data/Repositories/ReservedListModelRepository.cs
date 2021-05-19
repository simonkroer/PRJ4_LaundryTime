using LaundryTime.Data.Models.Booking;
using LaundryTime.Data.Repositories.RepositoryInterfaces;
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
    }
}
