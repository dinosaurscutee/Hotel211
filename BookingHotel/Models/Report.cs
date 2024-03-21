using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Report
    {
        public int ReportID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ReportName { get; set; }

        public string Description { get; set; }

        // Add this property to establish the relationship
        public ICollection<Booking> Bookings { get; set; }
    }
}
