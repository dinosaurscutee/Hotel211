using BookingHotel.Models;
using BookingHotel.Service;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BookingHotel.Controllers
{
    public class AccountController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly EmailService _emailService;

        public AccountController(HotelDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            DeleteExpiredTokens();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            // Kiểm tra xác thực đăng nhập
            bool isAuthenticated = AuthenticateUser(user.UserName, user.Password);

            if (isAuthenticated)
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var loggedInUser = _context.Users.SingleOrDefault(u => u.UserName == user.UserName);

                // Lấy tên vai trò của người dùng từ bảng Roles
                var roleName = _context.Roles.Where(r => r.RoleID == loggedInUser.RoleID).Select(r => r.RoleName).FirstOrDefault();

                // Lưu thông tin người dùng và vai trò vào session
                HttpContext.Session.SetString("UserName", loggedInUser.UserName);
                HttpContext.Session.SetString("UserRole", roleName);

                // Redirect đến trang chính sau khi đăng nhập thành công
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Đăng nhập thất bại, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Invalid username or password";
                return View(user);
            }
        }



        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            // Kiểm tra xem có tồn tại người dùng với email hoặc tên người dùng đã được đăng ký trước đó không
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == newUser.UserName || u.Email == newUser.Email);

            if (existingUser != null)
            {
                // Nếu đã tồn tại người dùng với email hoặc tên người dùng đã đăng ký trước đó, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "User with this email or username already exists. Please choose a different email or username.";
                return View(newUser);
            }

            if (!ModelState.IsValid)
            {
                // Tạo token ngẫu nhiên cho xác nhận email
                var token = GenerateRandomToken();

                // Lưu thông tin người dùng và token vào cơ sở dữ liệu
                newUser.IsEmailConfirmed = false;
                newUser.EmailConfirmationToken = token;
                newUser.RoleID = 3;
                newUser.TokenCreatedAt = DateTime.Now; // Lưu thời gian tạo token
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Gửi email xác nhận
                _emailService.SendConfirmationEmail(newUser.Email, token);

                // Chuyển hướng đến trang thông báo đăng ký thành công
                return RedirectToAction("RegistrationSuccess");
            }

            // Đăng ký không hợp lệ, hiển thị lại form đăng ký với thông báo lỗi
            return View(newUser);
        }



        public IActionResult EmailConfirmationSuccess()
        {
            return View();
        }


        public IActionResult ExpiredToken()
        {
            return View();
        }

        public IActionResult EmailConfirmationFailure()
        {
            return View();
        }
        public IActionResult ConfirmEmail(string email, string token)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.EmailConfirmationToken == token);
            if (user != null)
            {
                // Kiểm tra xem token có hết hạn không
                if (user.TokenCreatedAt == null)
                {
                    // Nếu token đã hết hạn, chuyển hướng đến trang thông báo token đã hết hạn
                    return RedirectToPage("ExpiredToken");
                }

                // Xác nhận email thành công
                user.IsEmailConfirmed = true;
                _context.SaveChanges();

                // Chuyển hướng đến trang thông báo xác nhận email thành công
                return RedirectToPage("EmailConfirmationSuccess");
            }

            // Xác nhận email thất bại, chuyển hướng đến trang thông báo lỗi
            return RedirectToPage("EmailConfirmationFailure");
        }

       





        // Hàm tạo token ngẫu nhiên
        private string GenerateRandomToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                token.Append(chars[random.Next(chars.Length)]);
            }
            return token.ToString();
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        private bool AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserName == username && u.Password == password);
            return user != null;
        }

        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Clear();

            // Chuyển hướng đến trang đăng nhập
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            // Retrieve the currently logged-in user
            var username = HttpContext.Session.GetString("UserName");
            var user = _context.Users.SingleOrDefault(u => u.UserName == username);

            if (user == null)
            {
                // User not found, handle accordingly (redirect, display error, etc.)
                return RedirectToAction("Login");
            }

            // Pass the user model to the view for editing
            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(User updatedUser)
        {
            //if (!ModelState.IsValid)
            //{
            //    // If model state is not valid, return the form with validation errors
            //    return View(updatedUser);
            //}

            // Retrieve the original user from the database
            var originalUser = _context.Users.SingleOrDefault(u => u.UserID == updatedUser.UserID);

            if (originalUser == null)
            {
                // Original user not found, handle accordingly
                return RedirectToAction("Login");
            }

            // Update the user properties with the new values
            originalUser.FirstName = updatedUser.FirstName;
            originalUser.LastName = updatedUser.LastName;
            originalUser.Email = updatedUser.Email;
            originalUser.PhoneNumber = updatedUser.PhoneNumber;

            // Save changes to the database
            _context.SaveChanges();

            // Redirect to a success page or any other appropriate action
            return RedirectToAction("Index", "Home");
        }

        // Trong AccountController

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu không
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Tạo token ngẫu nhiên cho việc reset mật khẩu
                var token = GenerateRandomToken();

                // Lưu token mới và thời gian tạo vào cơ sở dữ liệu
                user.ResetPasswordToken = token;
                user.TokenCreatedAt = DateTime.Now;
                _context.SaveChanges();

                // Gửi email chứa token cho người dùng
                _emailService.SendResetPasswordEmail(email, token);

                // Chuyển hướng đến trang thông báo gửi email thành công
                return RedirectToAction("PasswordResetEmailSent");
            }
            else
            {
                // Người dùng không tồn tại, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "User with this email does not exist.";
                return View();
            }
        }



        private void DeleteExpiredTokens()
        {
            var expiredTokens = _context.Users.Where(u => u.TokenCreatedAt < DateTime.Now.AddMinutes(-1)).ToList();

            foreach (var user in expiredTokens)
            {
                user.EmailConfirmationToken = null;
                user.ResetPasswordToken = null;
            }

            _context.SaveChanges();
        }




        public IActionResult PasswordResetEmailSent()
        {
            return View();
        }



        public IActionResult ForgotPasswordConfirmation(string email, string code)
        {
            // Kiểm tra xem có token từ URL không
            if (string.IsNullOrEmpty(code))
            {
                // Nếu không có token, chuyển hướng người dùng đến trang thông báo lỗi
                return RedirectToAction("Error", "Home");
            }

            // Kiểm tra xem token có hợp lệ không
            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.ResetPasswordToken == code);
            if (user != null)
            {
                // Nếu token hợp lệ, hiển thị trang reset mật khẩu
                return View();
            }
            else
            {
                // Nếu token không hợp lệ, chuyển hướng người dùng đến trang thông báo lỗi hoặc trang khác
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(string email, string password, string confirmPassword)
        {
            // Kiểm tra xác nhận mật khẩu
            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Password and Confirm Password do not match.";
                return View("ForgotPasswordConfirmation", email);
            }

            // Kiểm tra xem email có tồn tại trong cơ sở dữ liệu không
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Cập nhật mật khẩu mới cho người dùng
                user.Password = password;
                _context.SaveChanges();

                // Chuyển hướng đến trang thông báo reset mật khẩu thành công hoặc trang đăng nhập
                return RedirectToAction("PasswordResetSuccess");
            }
            else
            {
                // Người dùng không tồn tại, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "User with this email does not exist.";
                return View("ForgotPasswordConfirmation", email);
            }
        }
        public IActionResult PasswordResetSuccess()
        {
            return View();
        }


       



    }
}
