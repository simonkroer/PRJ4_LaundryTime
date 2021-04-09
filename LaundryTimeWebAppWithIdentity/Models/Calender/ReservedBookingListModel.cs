using System;

namespace LaundryTimeWebAppWithIdentity.Models.Calender
{
    public class ReservedBookingListModel
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Machine { get; set; }
        public DateTime Date { get; set; }
        public string UserFirstName { get; set; }
        public UserModel User { get; set; }
    }
}
