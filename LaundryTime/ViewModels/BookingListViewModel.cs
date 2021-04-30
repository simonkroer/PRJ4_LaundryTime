using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaundryTime.Models.Calender;

namespace LaundryTime.ViewModels
{
    public class BookingListViewModel
    {
        public string Time { get; set; }
        public DateTime Date { get; set; }
        public int MachineName { get; set; }
        public string MachineType { get; set; }
        public DatePickerModel DatePickerModel { get; set; }

    }
}
