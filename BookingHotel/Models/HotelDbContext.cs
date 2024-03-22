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
                new RoomStatus { StatusID = 3, StatusName = "Occupied" },
                new RoomStatus { StatusID = 4, StatusName = "Watting" }
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
                new User { UserID = 1, UserName = "admin", Password = "admin123", FirstName = "Admin", LastName = "User", Email = "admin@example.com", PhoneNumber = "123456789", RoleID = 1, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 2, UserName = "customer1", Password = "customer123", FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "987654321", RoleID = 2, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 3, UserName = "staff1", Password = "staff123", FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 4, UserName = "Dung", Password = "staff123", FirstName = "Ngo", LastName = "Dung", Email = "Dung@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 5, UserName = "Hieu", Password = "staff123", FirstName = "Hoang Quang", LastName = "Hieu", Email = "Hieu@example.com", PhoneNumber = "111222333", RoleID = 2, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 6, UserName = "Hoang", Password = "staff123", FirstName = "Nguyen", LastName = "Hoang", Email = "Hoang@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 7, UserName = "Khai", Password = "staff123", FirstName = "Hua", LastName = "Khai", Email = "Khai@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 8, UserName = "Lam", Password = "staff123", FirstName = "Tung", LastName = "Lam", Email = "Lam@example.com", PhoneNumber = "111222333", RoleID = 2, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 9, UserName = "Tuyen", Password = "staff123", FirstName = "Pham", LastName = "Tuyen", Email = "Tuyen@example.com", PhoneNumber = "111222333", RoleID = 3, EmailConfirmationToken = "abc", IsEmailConfirmed = true },
                new User { UserID = 10, UserName = "Huy", Password = "staff123", FirstName = "Quang", LastName = "Huy", Email = "Huy@example.com", PhoneNumber = "111222333", RoleID = 2, EmailConfirmationToken = "abc", IsEmailConfirmed = true }
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
                new RoomAmenities { AmenityID = 2, AmenityName = "TV", Description = "Flat-screen television" },
                new RoomAmenities { AmenityID = 3, AmenityName = "Mini Refrigerator", Description = "Mini bar includes free coffee and water" },
                new RoomAmenities { AmenityID = 4, AmenityName = "Air conditioning", Description = "Air conditioning and refreshment center" },
                new RoomAmenities { AmenityID = 5, AmenityName = "Lock the door securely", Description = "Coded secure door lock" },
                new RoomAmenities { AmenityID = 6, AmenityName = "Bathtub", Description = "Comfortable bathtub" }
            );

            // Seed data for HotelService
            modelBuilder.Entity<HotelService>().HasData(
                new HotelService { ServiceID = 1, ServiceName = "Room Service", Description = "In-room dining service" },
                new HotelService { ServiceID = 2, ServiceName = "Laundry", Description = "Laundry and dry cleaning service" },
                new HotelService { ServiceID = 3, ServiceName = "Nefflix", Description = "Nefflix and Chill" }
            );

            // Seed data for Room
            modelBuilder.Entity<Room>().HasData(
<<<<<<< HEAD
                new Room { RoomID = 1, RoomNumber = 101, RoomType = "Standard", StatusID = 1, Rate = 100, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 35000, Description= "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 2, RoomNumber = 102, RoomType = "Deluxe", StatusID = 1, Rate = 110, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 30000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 3, RoomNumber = 103, RoomType = "VIP", StatusID = 4, Rate = 120, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 45000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 4, RoomNumber = 104, RoomType = "Family", StatusID = 1, Rate = 180, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 40000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 5, RoomNumber = 105, RoomType = "Class", StatusID = 4, Rate = 130, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 50000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 6, RoomNumber = 106, RoomType = "Suite", StatusID = 1, Rate = 150, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 38000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 7, RoomNumber = 107, RoomType = "Suite", StatusID = 1, Rate = 170, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 38000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 8, RoomNumber = 108, RoomType = "Suite", StatusID = 4, Rate = 188, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 38000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." },
                new Room { RoomID = 9, RoomNumber = 109, RoomType = "Suite", StatusID = 1, Rate = 190, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", Price = 38000, Description = "The hotel lobby is a sanctuary of sophistication and relaxation, designed to provide a warm welcome to weary travelers and discerning guests alike. As you enter through the glass doors, you're greeted by the soft glow of ambient lighting, casting a gentle radiance upon the polished marble floors below. Plush velvet sofas and armchairs beckon invitingly, offering a comfortable respite for those seeking refuge from the hustle and bustle of the outside world." }
=======
                new Room { RoomID = 1, RoomNumber = 101, RoomType = "Standard", StatusID = 1, Rate = 100, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "Sea View", DescriptionRoom = "Standard 3-star room has an area of 45m2 and is equipped with 1 large, spacious and comfortable king bed. Guests staying at the hotel will be able to see Hoa Lac city with sea views overlooking 2 main roads. Not only that, with a bathroom full of equipment and services, customers will certainly have a feeling of relaxation and comfort when using it.", Price = 30000 },
                new Room { RoomID = 2, RoomNumber = 102, RoomType = "Deluxe", StatusID = 1, Rate = 150, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "Mountain View", DescriptionRoom = "3-star standard Deluxe room has an area of 45m2 equipped with 2 single beds (each bed size is 1.4m x 2m) very spacious and comfortable. Guests staying at the hotel will be able to see Hoa Lac city with a mountain view overlooking the majestic mountains. Not only that, with a bathroom full of equipment and services, customers will certainly have a feeling of relaxation and comfort when using it.", Price = 35000 },
                new Room { RoomID = 3, RoomNumber = 103, RoomType = "Family", StatusID = 1, Rate = 120, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "Mountain View", DescriptionRoom = "3-star standard Family room has an area of 45m2 equipped with 2 single beds (each bed size is 1.4m x 2m) very spacious and comfortable. Guests staying at the hotel will be able to see Hoa Lac city with a mountain view overlooking the majestic mountains. Not only that, with a bathroom full of equipment and services, customers will certainly have a feeling of relaxation and comfort when using it.", Price = 40000 },
                new Room { RoomID = 4, RoomNumber = 104, RoomType = "Super", StatusID = 1, Rate = 130, ServiceID = 1, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "City View", DescriptionRoom = "Super room meets 3-star standard with an area of 45m2 and a very elegant interior design, bringing a relaxing and gentle feeling when customers stay. The room has a view overlooking the spacious FPT University of Hoa Lac city, with a private balcony, customers will feel the bustle of this beautiful coastal city. The bedroom is equipped with 2 beds (each bed size is 1.2m x 2m) very comfortable. Besides, the spacious bathroom with full amenities helps customers relax and have a wonderful experience.", Price = 45000 },
                new Room { RoomID = 5, RoomNumber = 105, RoomType = "Suite", StatusID = 1, Rate = 140, ServiceID = 3, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "Sea View", DescriptionRoom = "This is also one of the two most spacious rooms (45m2) as well as the most luxurious in the hotel. The room has 3 beautiful views of Hoa Lac beach with 2 private balconies. The room has 1 bedroom (bed size is 2m x 2.2m) and 2 living areas: the living room and the tea area, suitable for receiving guests very formally and politely. The bathroom has a deep soaking tub that will help customers soak and relax after a tiring day of work.", Price = 50000 },
                new Room { RoomID = 6, RoomNumber = 106, RoomType = "VIP", StatusID = 1, Rate = 160, ServiceID = 2, ThumnailRoom = "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill", View = "City View", DescriptionRoom = "VIP Suite is one of the two largest rooms (45m2) as well as the most luxurious in the hotel. The room has 3 beautiful views overlooking the main streets of the city with 2 private balconies with great views. The room has 1 bedroom (bed size is 2m x 2.2m) and 2 living areas: the living room and the tea area that can receive guests very elegantly. Not only that, the bathroom has a deep soaking tub, which will help customers soak and relax after a tiring time.", Price = 55000 }
>>>>>>> 90c84bde4b459a521724f9668321bb7a91ed7ebb
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
