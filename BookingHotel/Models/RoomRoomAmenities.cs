namespace BookingHotel.Models
{
    public class RoomRoomAmenities
    {
        public int RoomID { get; set; }
        public Room Room { get; set; }

        public int AmenityID { get; set; }
        public RoomAmenities RoomAmenities { get; set; }
    }
}
