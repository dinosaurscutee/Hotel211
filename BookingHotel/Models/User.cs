using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public int? RoleID { get; set; }
        public Role Role { get; set; }

        [MaxLength(255)]
        public string? EmailConfirmationToken { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? TokenCreatedAt { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        public ICollection<EmployeeSchedule> EmployeeSchedules { get; set; }

        public ICollection<Payment> Payments { get; set; }

        // Quan hệ Một-Nhiều với Lịch sử Đặt phòng
        public ICollection<BookingHistory> BookingHistories { get; set; }
    }
}
