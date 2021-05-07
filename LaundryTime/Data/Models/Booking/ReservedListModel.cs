using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaundryTime.Data.Models.Booking
{
    public class ReservedListModel
    {
        public int Id { get; set; }
        public int OldId { get; set; }  
        public string Time { get; set; }
        public int MachineId { get; set; }
        public Machine Machine { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
