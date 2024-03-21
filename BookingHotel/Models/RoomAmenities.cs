using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class RoomAmenities
    {
        public int AmenityID { get; set; }

        [Required]
        [MaxLength(255)]
        public string AmenityName { get; set; }

        public string Description { get; set; }

        // Quan hệ Nhiều-Nhiều với Phòng
        public ICollection<RoomRoomAmenities> RoomRooms { get; set; }
    }
}
