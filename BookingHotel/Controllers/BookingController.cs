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


        [HttpPost]
        public IActionResult BookRoom(int roomId)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RoomID == roomId);

            if (room == null)
            {
                return NotFound();
            }

            decimal? roomPrice = (decimal)room.Price;

            // Generate a random number between 2 and 5 for CustomerID
            Random rnd = new Random();
            int customerId = rnd.Next(2, 6);

            var booking = new Booking
            {
                RoomID = roomId,
                CustomerID = customerId,
                CheckInDate = DateTime.Now,
                PaymentMethodID = 1,
                TotalAmount = roomPrice.HasValue ? Convert.ToDecimal(roomPrice.Value) : 0,
                ReportID = 1
            };

            // Thay đổi StatusID của phòng thành 2 (đã đặt)
            room.StatusID = 2;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Rooms.Update(room);

            // Thêm thông tin đặt phòng vào cơ sở dữ liệu
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            // Sau khi lưu thành công, bạn có thể chuyển hướng người dùng đến trang "Thank you" hoặc trang khác.
            return RedirectToAction("ThankYou");
        }


        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult ManageCheckouts()
        {
            // Lấy danh sách các đặt phòng chưa checkout
            var bookings = _context.Bookings
                .Include(b => b.Room)
                .Where(b => b.CheckOutDate == null)
                .ToList();

            return View(bookings);
        }

        // Action cập nhật thời gian checkout và trạng thái phòng
        [HttpPost]
        public IActionResult UpdateCheckoutTime(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            // Cập nhật thời gian checkout
            booking.CheckOutDate = DateTime.Now;

            // Cập nhật trạng thái của phòng về trạng thái trống (StatusID = 1)
            var room = _context.Rooms.Find(booking.RoomID);
            if (room != null)
            {
                room.StatusID = 1;
                _context.Rooms.Update(room);
            }

            _context.SaveChanges();

            return RedirectToAction("ManageCheckouts");
        }
    }
}
