using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Data.Models.Booking;

namespace LaundryTime.ViewModels
{
    public class BookingListViewModel
    {
        public int BookingID { get; set; }
        public string Time { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public int MachineName { get; set; }
        public string MachineType { get; set; }

    }
}
