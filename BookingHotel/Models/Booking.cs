using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public int? RoomID { get; set; }
        public Room Room { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
        public int? ReportID { get; set; }
        public Report Report { get; set; }


        public int PaymentMethodID { get; set; }
        public Payment PaymentMethod { get; set; }

        public ICollection<BookingDetails> BookingDetails { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
