using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class Role
    {
        public int RoleID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

        // Quan hệ Một-Nhiều với Người dùng
        public ICollection<User> Users { get; set; }
    }
}
