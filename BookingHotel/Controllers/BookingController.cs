using BookingHotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookingHotel.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelDbContext _context;

        public BookingController(HotelDbContext context)
        {
            _context = context;
        }

        // Action hiển thị trang xác nhận booking
        [HttpGet]
        public IActionResult ConfirmBooking(int id)
        {
            var room = _context.Rooms
                .Include(r => r.RoomAmenities)
                    .ThenInclude(ra => ra.RoomAmenities)
                .FirstOrDefault(r => r.RoomID == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // Action xử lý lưu thông tin booking vào cơ sở dữ liệu
        [HttpPost]
        public IActionResult BookRoom(int roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == roomId);

            if (room == null)
            {
                return NotFound();
            }

            decimal? roomPrice = (decimal)room.Price; // Ép kiểu từ double? sang decimal?

            var booking = new Booking
            {
                RoomID = roomId,
                CheckInDate = DateTime.Now,
                PaymentMethodID = 1,
                TotalAmount = roomPrice.HasValue ? Convert.ToDecimal(roomPrice.Value) : 0,
                ReportID = 1 
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Sau khi lưu thành công, bạn có thể chuyển hướng người dùng đến trang "Thank you" hoặc trang khác.
            return RedirectToAction("ThankYou");
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
