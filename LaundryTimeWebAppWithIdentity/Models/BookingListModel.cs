using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class BookingListModel
    {
        public IEnumerable<string> SelectedTimes { get; set; }
        public IEnumerable<SelectListItem> Times { get; set; }  
    }
}
