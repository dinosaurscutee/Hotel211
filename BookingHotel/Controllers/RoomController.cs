using BookingHotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class RoomController : Controller
{
	private readonly ILogger<RoomController> _logger;
	private readonly HotelDbContext _context;

	public RoomController(ILogger<RoomController> logger, HotelDbContext context)
	{
		_logger = logger;
		_context = context;
	}

	public IActionResult Rooms(string roomType, double? minPrice, double? maxPrice)
	{
		IQueryable<Room> roomsQuery = _context.Rooms.Include(r => r.Status);

		// Áp dụng các điều kiện lọc
		if (!string.IsNullOrEmpty(roomType))
		{
			roomsQuery = roomsQuery.Where(r => r.RoomType == roomType);
		}

		if (minPrice.HasValue || maxPrice.HasValue)
		{
			roomsQuery = roomsQuery.Where(r => (double)r.Price >= minPrice && (double)r.Price <= maxPrice);
		}

		var rooms = roomsQuery.ToList();

		// Lưu danh sách Room Types vào ViewBag
		ViewBag.RoomTypes = _context.Rooms.Select(r => r.RoomType).Distinct().ToList();

		return View(rooms);
	}
}
