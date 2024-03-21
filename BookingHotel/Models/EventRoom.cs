using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class EventRoom
    {
        public int EventRoomID { get; set; }

        public int? RoomID { get; set; }
        public Room Room { get; set; }

        [Required]
        [MaxLength(255)]
        public string EventName { get; set; }

        public string EventDescription { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string Amenities { get; set; }
    }
}
