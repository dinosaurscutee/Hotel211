using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class BookingHistory
    {
        public int BookingHistoryID { get; set; }

        public int? CustomerID { get; set; }
        public User Customer { get; set; }

        public int? BookingID { get; set; }
        public Booking Booking { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }
}
