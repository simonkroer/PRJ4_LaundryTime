using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class DateModel
    {
        public int Id { get; set; }
        public DateTime Datedata { get; set; }

        public List<BookingListModel> BookingListModels { get; set; }
    }
}
