﻿using BookingHotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace BookingHotel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly HotelDbContext _context;

        public HomeController(ILogger<HomeController> logger, HotelDbContext context)

        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var rooms = _context.Rooms
                .Where(p=>p.StatusID == 1)
                .ToList();
            return View(rooms);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
