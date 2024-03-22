using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookingHotel.Models;

namespace BookingHotel.Controllers
{
    public class EmployeeSchedulesController : Controller
    {
        private readonly HotelDbContext _context;

        public EmployeeSchedulesController(HotelDbContext context)
        {
            _context = context;
        }

        // GET: EmployeeSchedules
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                // Nếu không phải là Admin, chuyển hướng đến trang đăng nhập hoặc trang lỗi khác
                return RedirectToAction("Login", "Account"); // Đây là giả định về tên của controller và action đăng nhập của bạn
            }
            var hotelDbContext = _context.EmployeeSchedules.Include(e => e.Employee);
            return View(await hotelDbContext.ToListAsync());
        }

        // GET: EmployeeSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.EmployeeSchedules == null)
            {
                return NotFound();
            }

            var employeeSchedule = await _context.EmployeeSchedules
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
            if (employeeSchedule == null)
            {
                return NotFound();
            }

            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                // Nếu không phải là Admin, chuyển hướng đến trang đăng nhập hoặc trang lỗi khác
                return RedirectToAction("Login", "Account"); // Đây là giả định về tên của controller và action đăng nhập của bạn
            }
            // Lấy danh sách người dùng có vai trò là 2 từ cơ sở dữ liệu
            var usersWithRole2 = _context.Users.Where(u => u.RoleID == 2).ToList();

            // Tạo danh sách thả xuống từ danh sách người dùng
            ViewData["EmployeeID"] = new SelectList(usersWithRole2, "UserID", "UserName");

            return View();
        }


        // POST: EmployeeSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduleID,EmployeeID,Taskname,Slot,ShiftStartTime,ShiftEndTime")] EmployeeSchedule employeeSchedule)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                // Nếu không phải là Admin, chuyển hướng đến trang đăng nhập hoặc trang lỗi khác
                return RedirectToAction("Login", "Account"); // Đây là giả định về tên của controller và action đăng nhập của bạn
            }
            //if (ModelState.IsValid)
            //{
            _context.Add(employeeSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["EmployeeID"] = new SelectList(_context.Users, "UserID", "UserName", employeeSchedule.EmployeeID);
            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeSchedule = await _context.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule == null)
            {
                return NotFound();
            }

            // Lấy danh sách người dùng có RoleID = 2
            var usersWithRoleId2 = _context.Users.Where(u => u.RoleID == 2);
            ViewData["EmployeeID"] = new SelectList(usersWithRoleId2, "UserID", "UserName", employeeSchedule.EmployeeID);

            return View(employeeSchedule);
        }


        // POST: EmployeeSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleID,EmployeeID,Taskname,Slot,ShiftStartTime,ShiftEndTime")] EmployeeSchedule employeeSchedule)
        {
            if (id != employeeSchedule.ScheduleID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(employeeSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeScheduleExists(employeeSchedule.ScheduleID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            ViewData["EmployeeID"] = new SelectList(_context.Users, "UserID", "UserName", employeeSchedule.EmployeeID);
            return View(employeeSchedule);
        }

        // GET: EmployeeSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.EmployeeSchedules == null)
            {
                return NotFound();
            }

            var employeeSchedule = await _context.EmployeeSchedules
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.ScheduleID == id);
            if (employeeSchedule == null)
            {
                return NotFound();
            }

            return View(employeeSchedule);
        }

        // POST: EmployeeSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.EmployeeSchedules == null)
            {
                return Problem("Entity set 'HotelDbContext.EmployeeSchedules'  is null.");
            }
            var employeeSchedule = await _context.EmployeeSchedules.FindAsync(id);
            if (employeeSchedule != null)
            {
                _context.EmployeeSchedules.Remove(employeeSchedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeScheduleExists(int id)
        {
          return (_context.EmployeeSchedules?.Any(e => e.ScheduleID == id)).GetValueOrDefault();
        }
    }
}
