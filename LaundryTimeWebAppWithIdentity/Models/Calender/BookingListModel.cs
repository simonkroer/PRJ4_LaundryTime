namespace LaundryTimeWebAppWithIdentity.Models.Calender
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
