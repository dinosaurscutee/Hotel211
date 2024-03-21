using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }

        public int? CustomerID { get; set; }
        public User Customer { get; set; }

        public int? BookingID { get; set; }
        public Booking Booking { get; set; }

        [Required]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
