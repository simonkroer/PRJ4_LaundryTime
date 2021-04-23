using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models.Booking
{
    public class DateModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<BookingListModel> BookingListModels { get; set; }
        public List<ReservedListModel> ReservedListModels { get; set; }

    }
}
