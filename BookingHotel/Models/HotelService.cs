﻿using System.ComponentModel.DataAnnotations;

namespace BookingHotel.Models
{
    public class HotelService
    {
        public int ServiceID { get; set; }

        [Required]
        [MaxLength(255)]
        public string ServiceName { get; set; }

        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Price { get; set; }
        public string? Rating { get; set; }

        // Quan hệ Một-Nhiều với Phòng
        public ICollection<Room> Rooms { get; set; }
    }
}
