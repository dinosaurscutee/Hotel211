using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Task
    {
        public int TaskID { get; set; }

        public int? RoomID { get; set; }
        public Room Room { get; set; }

        public string TaskDescription { get; set; }

        [Required]
        public DateTime ScheduledTime { get; set; }

        public string TaskStatus { get; set; }
    }
}
