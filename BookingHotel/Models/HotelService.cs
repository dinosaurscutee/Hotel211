using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class HotelService
    {
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ServiceName { get; set; }

        public string Description { get; set; }

        // Quan hệ Một-Nhiều với Phòng
        public ICollection<Room> Rooms { get; set; }
    }
}
