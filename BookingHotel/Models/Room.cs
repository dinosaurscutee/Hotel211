using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Room
    {
        public int RoomID { get; set; }

        [Required]
        public int RoomNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public string RoomType { get; set; }

        public int? StatusID { get; set; }
        public RoomStatus Status { get; set; }

        [Required]
        public decimal Rate { get; set; }
        public int ServiceID { get; set; }

        public string ThumnailRoom { get; set; }

        public double? Price { get; set; }

        public string View { get; set; }

        public string DescriptionRoom { get; set; }

        // Quan hệ Nhiều-Nhiều với Tiện ích Phòng
        public ICollection<RoomRoomAmenities> RoomAmenities { get; set; }
        public HotelService Service { get; set; }
        public ICollection<EventRoom> EventRooms { get; set; }
        public ICollection<Task> HousekeepingTasks { get; set; }
    }
}
