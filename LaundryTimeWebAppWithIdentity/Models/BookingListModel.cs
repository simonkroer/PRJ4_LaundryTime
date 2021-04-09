using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaundryTimeWebAppWithIdentity.Models
{
    public class BookingListModel
    {
        public int Id { get; set; } 
        public string Status { get; set; }
        public string Time { get; set; }
        public string Machine { get; set; }

        public DateModel DateModel { get; set; }
    }
}
