using System;
using System.Collections.Generic;

namespace LaundryTimeWebAppWithIdentity.Models.Calender
{
    public class DateModel
    {
        public int Id { get; set; }
        public DateTime Datedata { get; set; }

        public List<BookingListModel> BookingListModels { get; set; }
    }
}
