namespace BookingHotel.Models
{
    public class BookingDetails
    {
        public int BookingDetailID { get; set; }

        public int? BookingID { get; set; }
        public Booking Booking { get; set; }

        public int? RoomID { get; set; }
        public Room Room { get; set; }

        public string SpecialRequests { get; set; }
    }
}
