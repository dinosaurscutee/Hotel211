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

        if (!string.IsNullOrEmpty(roomType))
        {
            roomsQuery = roomsQuery.Where(r => r.RoomType == roomType);
        }

        if (minPrice.HasValue || maxPrice.HasValue)
        {
            roomsQuery = roomsQuery.Where(r => (double)r.Price >= minPrice && (double)r.Price <= maxPrice);
        }

        var rooms = roomsQuery.ToList();

        ViewBag.RoomTypes = _context.Rooms.Select(r => r.RoomType).Distinct().ToList();

        return View(rooms);
    }

    public IActionResult RoomDetail(int id)
    {
        var room = _context.Rooms
            .Include(r => r.RoomAmenities)
                .ThenInclude(ra => ra.RoomAmenities)
            .FirstOrDefault(r => r.RoomID == id);

        if (room == null)
        {
            return NotFound();
        }

        var otherRooms = _context.Rooms.Where(r => r.RoomID != id).ToList();
        ViewBag.Room = room;
        ViewBag.OtherRooms = otherRooms;

        return View(room);
    }



}
