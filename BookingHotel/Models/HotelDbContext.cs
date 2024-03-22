using Microsoft.EntityFrameworkCore;

namespace BookingHotel.Models
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options) : base(options)
        {
        }

        public HotelDbContext()
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<RoomAmenities> RoomAmenities { get; set; }
        public DbSet<RoomRoomAmenities> RoomRoomAmenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetails> BookingDetails { get; set; }
        public DbSet<Payment> PaymentMethods { get; set; }
        public DbSet<HotelService> HotelServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BookingHistory> BookingHistories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        public DbSet<EventRoom> EventRooms { get; set; }
        public DbSet<Task> HousekeepingTasks { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingDetails>()
                .HasKey(bd => bd.BookingDetailID);

            modelBuilder.Entity<EmployeeSchedule>()
               .HasKey(es => es.ScheduleID);
            modelBuilder.Entity<HotelService>()
              .HasKey(es => es.ServiceID);  
            modelBuilder.Entity<Task>()
              .HasKey(es => es.TaskID);
            modelBuilder.Entity<RoomAmenities>()
              .HasKey(es => es.AmenityID);
            modelBuilder.Entity<RoomStatus>()
              .HasKey(es => es.StatusID);


            // Cấu hình quan hệ Nhiều-Nhiều giữa Phòng và Tiện ích Phòng
            modelBuilder.Entity<RoomRoomAmenities>()
                .HasKey(rra => new { rra.RoomID, rra.AmenityID });

            modelBuilder.Entity<RoomRoomAmenities>()
                .HasOne(rra => rra.Room)
                .WithMany(r => r.RoomAmenities)
                .HasForeignKey(rra => rra.RoomID);

            modelBuilder.Entity<RoomRoomAmenities>()
                .HasOne(rra => rra.RoomAmenities)
                .WithMany(ra => ra.RoomRooms)
                .HasForeignKey(rra => rra.AmenityID);

            // Cấu hình quan hệ Một-Nhiều giữa Trạng thái Phòng và Phòng
            modelBuilder.Entity<RoomStatus>()
                .HasMany(rs => rs.Rooms)
                .WithOne(r => r.Status)
                .HasForeignKey(r => r.StatusID);

            // Cấu hình quan hệ Một-Nhiều giữa Đặt phòng và Chi tiết Đặt phòng
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.BookingDetails)
                .WithOne(bd => bd.Booking)
                .HasForeignKey(bd => bd.BookingID);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Report)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.ReportID);

            // Cấu hình quan hệ Một-Nhiều giữa Phương thức thanh toán và Đặt phòng
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.PaymentMethod)
                .WithMany(pm => pm.Bookings)
                .HasForeignKey(b => b.PaymentMethodID);

            // Cấu hình quan hệ Một-Nhiều giữa Dịch vụ Khách sạn và Phòng
            modelBuilder.Entity<HotelService>()
                .HasMany(hs => hs.Rooms)
                .WithOne(r => r.Service)
                .HasForeignKey(r => r.ServiceID);

            // Cấu hình quan hệ Một-Nhiều giữa Người dùng và Lịch sử Đặt phòng
            modelBuilder.Entity<User>()
                .HasMany(u => u.BookingHistories)
                .WithOne(bh => bh.Customer)
                .HasForeignKey(bh => bh.CustomerID);

            // Cấu hình quan hệ Một-Nhiều giữa Vai trò và Người dùng
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleID);

            // Cấu hình quan hệ Một-Nhiều giữa Người dùng và Lịch làm việc nhân viên
            modelBuilder.Entity<User>()
                .HasMany(u => u.EmployeeSchedules)
                .WithOne(es => es.Employee)
                .HasForeignKey(es => es.EmployeeID);

            // Cấu hình quan hệ Nhiều-Nhiều giữa Phòng và Sự kiện
            modelBuilder.Entity<Room>()
                .HasMany(r => r.EventRooms)
                .WithOne(er => er.Room)
                .HasForeignKey(er => er.RoomID);

            // Cấu hình quan hệ Nhiều-Nhiều giữa Phòng và Lịch làm việc nhân viên
            modelBuilder.Entity<Room>()
                .HasMany(r => r.HousekeepingTasks)
                .WithOne(ht => ht.Room)
                .HasForeignKey(ht => ht.RoomID);

            // Cấu hình quan hệ Một-Nhiều giữa Đặt phòng và Phản hồi
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Feedbacks)
                .WithOne(f => f.Booking)
                .HasForeignKey(f => f.BookingID);

            // Các cấu hình khác về quan hệ giữa các bảng có thể được thêm vào tùy thuộc vào yêu cầu cụ thể của bạn
            seedData(modelBuilder);
        }

        private void seedData(ModelBuilder modelBuilder)
        {
            // Seed data for RoomStatus
            modelBuilder.Entity<RoomStatus>().HasData(
                new RoomStatus { StatusID = 1, StatusName = "Available" },
                new RoomStatus { StatusID = 2, StatusName = "Booked" },
                new RoomStatus { StatusID = 3, StatusName = "Occupied" }
            );

            // Seed data for Payment
            modelBuilder.Entity<Payment>().HasData(
                new Payment { PaymentID = 1, PaymentMethodName = "Credit Card", PaymentAmount = 0, PaymentDate = DateTime.Now },
                new Payment { PaymentID = 2, PaymentMethodName = "Cash", PaymentAmount = 0, PaymentDate = DateTime.Now },
                new Payment { PaymentID = 3, PaymentMethodName = "PayPal", PaymentAmount = 0, PaymentDate = DateTime.Now }
            );

            // Seed data for Report
            modelBuilder.Entity<Report>().HasData(
                new Report { ReportID = 1, ReportName = "Room Cleanliness", Description = "Cleanliness report for rooms" },
                new Report { ReportID = 2, ReportName = "Customer Satisfaction", Description = "Customer satisfaction report" }
            );

            // Seed data for Role
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, RoleName = "Admin" },
                new Role { RoleID = 2, RoleName = "Customer" },
                new Role { RoleID = 3, RoleName = "Staff" }
            );

            // Seed data for User
            modelBuilder.Entity<User>().HasData(
                new User { UserID = 1, UserName = "admin", Password = "admin123", FirstName = "Admin", LastName = "User", Email = "admin@example.com", PhoneNumber = "123456789", RoleID = 1,EmailConfirmationToken = "abc" , IsEmailConfirmed = true },
                new User { UserID = 2, UserName = "customer1", Password = "customer123", FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "987654321", RoleID = 2, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 3, UserName = "staff1", Password = "staff123", FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true }
            );

            // Seed data for Booking
            modelBuilder.Entity<Booking>().HasData(
                new Booking { BookingID = 1, CustomerID = 2, RoomID = 1, CheckInDate = DateTime.Now.AddDays(1), CheckOutDate = DateTime.Now.AddDays(3), TotalAmount = 200, ReportID = 1, PaymentMethodID = 1 },
                new Booking { BookingID = 2, CustomerID = 3, RoomID = 2, CheckInDate = DateTime.Now.AddDays(2), CheckOutDate = DateTime.Now.AddDays(4), TotalAmount = 300, ReportID = 2, PaymentMethodID = 2 }
            );

            // Seed data for BookingDetails
            modelBuilder.Entity<BookingDetails>().HasData(
                new BookingDetails { BookingDetailID = 1, BookingID = 1, RoomID = 1, SpecialRequests = "Extra pillows" },
                new BookingDetails { BookingDetailID = 2, BookingID = 2, RoomID = 2, SpecialRequests = "Early check-in" }
            );

            // Seed data for Feedback
            modelBuilder.Entity<Feedback>().HasData(
                new Feedback { FeedbackID = 1, CustomerID = 2, BookingID = 1, Rating = 5, Comment = "Great experience!" },
                new Feedback { FeedbackID = 2, CustomerID = 3, BookingID = 2, Rating = 4, Comment = "Could be better" }
            );

            // Seed data for RoomAmenities
            modelBuilder.Entity<RoomAmenities>().HasData(
                new RoomAmenities { AmenityID = 1, AmenityName = "WiFi", Description = "High-speed internet access" },
                new RoomAmenities { AmenityID = 2, AmenityName = "TV", Description = "Flat-screen television" }
            );

            // Seed data for HotelService
            modelBuilder.Entity<HotelService>().HasData(
                new HotelService
                {
                    ServiceID = 1,
                    ServiceName = "Spa and Massage",
                    Description = "Providing massage therapies and spa services to help customers relax and regenerate energy",
                    ImageUrl = "https://www.serenevillas.com/images/mainpic-spa.jpg",
                    Price = null,
                    Rating = "4.00"
                },
                new HotelService
                {
                    ServiceID = 2,
                    ServiceName = "Travel support",
                    Description = "Providing travel information, booking tours, sightseeing tickets, shuttle buses, etc",
                    ImageUrl = "https://bcp.cdnchinhphu.vn/334894974524682240/2023/10/3/dulich-16963249467831857858889.jpg",
                    Price = 2000000.00m,
                    Rating = "5.00"
                },

                new HotelService
                {
                    ServiceID = 3,
                    ServiceName = "Room service",
                    Description = "Includes cleaning the room, changing bed sheets, providing drinking water, personal items, and any other requests from the customer",
                    ImageUrl = "https://www.huongnghiepaau.com/wp-content/uploads/2020/08/room-service-la-gi-1.jpg",
                    Price = 200000.00m,
                    Rating = "4.00"
                },
                new HotelService
                {
                    ServiceID = 4,
                    ServiceName = "Free WiFi",
                    Description = "Service that provides wireless internet connection to customers throughout the hotel area",
                    ImageUrl = "https://www.smartcity.co.nz/wp-content/uploads/2023/04/why-should-we-have-free-public-wi-fi.jpg",
                    Price = 75000.00m,
                    Rating = "3.00"
                },
                new HotelService
                {
                    ServiceID = 5,
                    ServiceName = "Restaurant service",
                    Description = "A place that provides food and drinks to customers, including breakfast, lunch, and dinner",
                    ImageUrl = "https://phongcachmoc.vn/upload/images/tin-tuc/20%20mau%20nha%20hang%20dep/update-07-2022/Sushi-World-Ton-That-Thiep-10.JPG",
                    Price = 150000.00m,
                    Rating = "4.50"
                },
                new HotelService
                {
                    ServiceID = 6,
                    ServiceName = "Gym and swimming",
                    Description = "Some hotels provide recreational facilities such as swimming pools and gyms for guests",
                    ImageUrl = "https://mirefoot.co.uk/wp-content/uploads/2019/05/Mierfoot-Pool-05-resized-850x567.jpg",
                    Price = 200000.00m,
                    Rating = "2.50"
                },
                new HotelService
                {
                    ServiceID = 7,
                    ServiceName = "Airport pick up",
                    Description = "Provides shuttle service from the airport to the hotel and vice versa",
                    ImageUrl = "https://jugnoo.io/wp-content/uploads/2021/05/1-6-1024x543.png",
                    Price = 1000000.00m,
                    Rating = "4.00"
                },
                new HotelService
                {
                    ServiceID = 8,
                    ServiceName = "Meeting room service",
                    Description = "Some hotels have conference and wedding rooms to organize events",
                    ImageUrl = "https://cdn0.weddingwire.com/article/1435/3_2/960/jpg/15341-communicate-with-vendors-rawpixel.jpeg",
                    Price = 750000.00m,
                    Rating = "2.50"
                }
            );

            // Seed data for Room
            modelBuilder.Entity<Room>().HasData(
                new Room { RoomID = 1, RoomNumber = 101, RoomType = "Standard", StatusID = 1, Rate = 100, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 35000, Description= "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 2, RoomNumber = 102, RoomType = "Deluxe", StatusID = 1, Rate = 110, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 30000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 3, RoomNumber = 103, RoomType = "VIP", StatusID = 1, Rate = 120, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 45000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 4, RoomNumber = 104, RoomType = "Family", StatusID = 1, Rate = 180, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 40000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 5, RoomNumber = 105, RoomType = "Class", StatusID = 1, Rate = 130, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 50000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 6, RoomNumber = 106, RoomType = "Suite", StatusID = 1, Rate = 150, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 38000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." }
            );

            // Seed data for EmployeeSchedule
            modelBuilder.Entity<EmployeeSchedule>().HasData(
                new EmployeeSchedule { ScheduleID = 1, EmployeeID = 3, ShiftStartTime = DateTime.Parse("08:00 AM"), ShiftEndTime = DateTime.Parse("04:00 PM") }
            );

            // Seed data for EventRoom
            modelBuilder.Entity<EventRoom>().HasData(
                new EventRoom { EventRoomID = 1, RoomID = 2, EventName = "Conference", EventDescription = "Corporate conference", EventDate = DateTime.Now.AddDays(7), Amenities = "Projector, Whiteboard" }
            );

            // Seed data for Task
            modelBuilder.Entity<Task>().HasData(
                new Task { TaskID = 1, RoomID = 1, TaskDescription = "Clean room", ScheduledTime = DateTime.Now.AddDays(1), TaskStatus = "Pending" },
                new Task { TaskID = 2, RoomID = 2, TaskDescription = "Change beddings", ScheduledTime = DateTime.Now.AddDays(2), TaskStatus = "Pending" }
            );

    
        }

    }
}
