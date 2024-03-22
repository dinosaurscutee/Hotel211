using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Models;
using System.Globalization;

namespace BookingHotel.Controllers
{
    public class ReportingAnalysisController : Controller
    {
        private readonly HotelDbContext context;

        public ReportingAnalysisController(HotelDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int? year)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                // Nếu không phải là Admin, chuyển hướng đến trang đăng nhập hoặc trang lỗi khác
                return RedirectToAction("Login", "Account"); // Đây là giả định về tên của controller và action đăng nhập của bạn
            }
            List<Booking> bookings1 = context.Bookings.ToList();
            int countBook = bookings1.Count;
            decimal sumtotal = 0;
            foreach(Booking booking1 in bookings1)
            {
                sumtotal += booking1.TotalAmount;
            }
            ViewBag.countBook = countBook;
            ViewBag.SumTotal = sumtotal;
            // Nếu year không được chỉ định, sẽ sử dụng năm hiện tại
            int selectedYear = year ?? DateTime.Now.Year;

            // Lấy danh sách các booking từ cơ sở dữ liệu cho năm được chọn
            List<Booking> bookings = context.Bookings.Where(b => b.CheckInDate.Year == selectedYear).ToList();

            // Khởi tạo một Dictionary để lưu tổng thu nhập từng tháng
            Dictionary<string, decimal> incomeByMonth = new Dictionary<string, decimal>();

            // Lặp qua từng booking để tính tổng thu nhập từng tháng
            foreach (Booking booking in bookings)
            {
                // Lấy tháng và năm của CheckInDate
                string monthYear = $"{booking.CheckInDate.Month}/{booking.CheckInDate.Year}";

                // Nếu Dictionary chưa chứa key tương ứng với tháng và năm hiện tại
                if (!incomeByMonth.ContainsKey(monthYear))
                {
                    // Thêm key mới vào Dictionary và gán giá trị là TotalAmount của booking hiện tại
                    incomeByMonth[monthYear] = booking.TotalAmount;
                }
                else
                {
                    // Nếu key đã tồn tại, cộng thêm giá trị TotalAmount của booking hiện tại vào giá trị hiện có
                    incomeByMonth[monthYear] += booking.TotalAmount;
                }
            }

            // Sắp xếp thứ tự các tháng từ nhỏ đến lớn
            var sortedIncomeByMonth = incomeByMonth.OrderBy(x => DateTime.ParseExact(x.Key, "M/yyyy", CultureInfo.InvariantCulture));

            // Trả về View và truyền Dictionary incomeByMonth vào để hiển thị
            return View(sortedIncomeByMonth.ToDictionary(x => x.Key, x => x.Value));
        }

        public IActionResult TopBooking()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                // Nếu không phải là Admin, chuyển hướng đến trang đăng nhập hoặc trang lỗi khác
                return RedirectToAction("Login", "Account"); // Đây là giả định về tên của controller và action đăng nhập của bạn
            }
            var topRooms = context.Bookings
                .GroupBy(b => b.RoomID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();

            // Lấy thông tin chi tiết của các phòng top
            var topRoomsDetails = context.Rooms
                .Where(r => topRooms.Contains(r.RoomID))
                .ToList();

            // Tạo một Dictionary để lưu thông tin số lần book và tổng số tiền
            var roomStatistics = new Dictionary<int, Tuple<int, decimal>>();

            foreach (var room in topRoomsDetails)
            {
                var bookings = context.Bookings.Where(b => b.RoomID == room.RoomID).ToList();
                var totalBookings = bookings.Count();
                var totalRevenue = bookings.Sum(b => b.TotalAmount);
                roomStatistics.Add(room.RoomID, new Tuple<int, decimal>(totalBookings, totalRevenue));
            }

            // Sắp xếp danh sách topRoomsDetails dựa trên tổng doanh thu mỗi phòng
            topRoomsDetails = topRoomsDetails.OrderByDescending(r => roomStatistics[r.RoomID].Item2).ToList();

            ViewBag.RoomStatistics = roomStatistics;

            return View(topRoomsDetails);
        }

    }
}
