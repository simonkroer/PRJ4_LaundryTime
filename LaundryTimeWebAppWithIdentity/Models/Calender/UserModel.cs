using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace LaundryTimeWebAppWithIdentity.Models.Calender
{
    public class UserModel : IdentityUser
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<ReservedBookingListModel> reservedBooking { get; set; }
    }
}
