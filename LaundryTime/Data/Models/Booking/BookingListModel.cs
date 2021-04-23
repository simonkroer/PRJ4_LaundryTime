using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models.Booking
{
    public class BookingListModel
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string Time { get; set; }

        public int MachineName { get; set; }
        public Machine Machine { get; set; }
        
        public DateTime Date { get; set; }
        public DateModel DateModel { get; set; }

    }
}
