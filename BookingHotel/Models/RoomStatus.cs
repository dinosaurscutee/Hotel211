using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class RoomStatus
    {
        public int StatusID { get; set; }

        [Required]
        [MaxLength(50)]
        public string StatusName { get; set; }

        // Quan hệ Một-Nhiều với Phòng
        public ICollection<Room> Rooms { get; set; }
    }
}
