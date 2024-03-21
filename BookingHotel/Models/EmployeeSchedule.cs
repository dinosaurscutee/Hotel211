using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class EmployeeSchedule
    {
        public int ScheduleID { get; set; }

        public int? EmployeeID { get; set; }
        public string? Taskname { get; set; }
        public string? Slot { get; set; }
        public User Employee { get; set; }

        [Required]
        public DateTime ShiftStartTime { get; set; }

        [Required]
        public DateTime ShiftEndTime { get; set; }


    }
}
