﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Models;

namespace BookingHotel.Controllers
{
    public class CleanCheckoutController : Controller
    {
        private readonly HotelDbContext _context;
        public CleanCheckoutController(HotelDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index(int id)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                // Lấy vai trò của người dùng từ session
                string userRole = HttpContext.Session.GetString("UserRole");

                // Kiểm tra xem vai trò của người dùng có phải là Admin hay không
                if (userRole == "Admin")
                {
                    List<Room> rooms = _context.Rooms.Where(x => x.StatusID == 4).ToList();
                    List<User> users = _context.Users.Where(x => x.RoleID == 2).ToList();

                    // Kiểm tra nếu id của phòng được truyền vào khác null
                    if (id != null)
                    {
                        // Tìm phòng có id tương ứng và cập nhật StatusID thành 1
                        Room roomToUpdate = rooms.FirstOrDefault(r => r.RoomID == id);
                        if (roomToUpdate != null)
                        {
                            roomToUpdate.StatusID = 1;
                            _context.SaveChanges();
                        }
                    }

                    rooms = _context.Rooms.Where(x => x.StatusID == 4).ToList();
                    ViewBag.Rooms = rooms;
                    ViewBag.Users = users;
                    return View();
                }
            }

            // Nếu người dùng không phải là Admin hoặc chưa đăng nhập, chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login", "Account");
        }


    }
}
