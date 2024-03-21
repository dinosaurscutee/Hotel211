using System.ComponentModel.DataAnnotations;
using BookingHotel.Models;

public class Payment
{
    public int PaymentID { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethodName { get; set; }

    public decimal PaymentAmount { get; set; }
    public DateTime PaymentDate { get; set; }

    // Quan hệ Một-Nhiều với Đặt phòng
    public ICollection<Booking> Bookings { get; set; }
}
