using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models.Booking
{
    public class ReservedListModel
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Machine { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
