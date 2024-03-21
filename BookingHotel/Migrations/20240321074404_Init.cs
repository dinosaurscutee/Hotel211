using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingHotel.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HotelServices",
                columns: table => new
                {
                    ServiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelServices", x => x.ServiceID);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "RoomAmenities",
                columns: table => new
                {
                    AmenityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmenityName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomAmenities", x => x.AmenityID);
                });

            migrationBuilder.CreateTable(
                name: "RoomStatuses",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    EmailConfirmationToken = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<int>(type: "int", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceID = table.Column<int>(type: "int", nullable: false),
                    ThumnailRoom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Rooms_HotelServices_ServiceID",
                        column: x => x.ServiceID,
                        principalTable: "HotelServices",
                        principalColumn: "ServiceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "RoomStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeSchedules",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(type: "int", nullable: false),
                    ShiftStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShiftEndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeSchedules", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_EmployeeSchedules_Users_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "EventRooms",
                columns: table => new
                {
                    EventRoomID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EventDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amenities = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRooms", x => x.EventRoomID);
                    table.ForeignKey(
                        name: "FK_EventRooms_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HousekeepingTasks",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaskStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HousekeepingTasks", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_HousekeepingTasks_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRoomAmenities",
                columns: table => new
                {
                    RoomID = table.Column<int>(type: "int", nullable: false),
                    AmenityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoomAmenities", x => new { x.RoomID, x.AmenityID });
                    table.ForeignKey(
                        name: "FK_RoomRoomAmenities_RoomAmenities_AmenityID",
                        column: x => x.AmenityID,
                        principalTable: "RoomAmenities",
                        principalColumn: "AmenityID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoomAmenities_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReportID = table.Column<int>(type: "int", nullable: false),
                    PaymentMethodID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_Bookings_PaymentMethods_PaymentMethodID",
                        column: x => x.PaymentMethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Reports_ReportID",
                        column: x => x.ReportID,
                        principalTable: "Reports",
                        principalColumn: "ReportID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    BookingDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: true),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.BookingDetailID);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID");
                });

            migrationBuilder.CreateTable(
                name: "BookingHistories",
                columns: table => new
                {
                    BookingHistoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    BookingID = table.Column<int>(type: "int", nullable: true),
                    CheckInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHistories", x => x.BookingHistoryID);
                    table.ForeignKey(
                        name: "FK_BookingHistories_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID");
                    table.ForeignKey(
                        name: "FK_BookingHistories_Users_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<int>(type: "int", nullable: true),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackID);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Bookings_BookingID",
                        column: x => x.BookingID,
                        principalTable: "Bookings",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.InsertData(
                table: "HotelServices",
                columns: new[] { "ServiceID", "Description", "ServiceName" },
                values: new object[,]
                {
                    { 1, "In-room dining service", "Room Service" },
                    { 2, "Laundry and dry cleaning service", "Laundry" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentID", "PaymentAmount", "PaymentDate", "PaymentMethodName", "UserID" },
                values: new object[,]
                {
                    { 1, 0m, new DateTime(2024, 3, 21, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2787), "Credit Card", null },
                    { 2, 0m, new DateTime(2024, 3, 21, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2799), "Cash", null },
                    { 3, 0m, new DateTime(2024, 3, 21, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2800), "PayPal", null }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "ReportID", "Description", "ReportName" },
                values: new object[,]
                {
                    { 1, "Cleanliness report for rooms", "Room Cleanliness" },
                    { 2, "Customer satisfaction report", "Customer Satisfaction" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleID", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Customer" },
                    { 3, "Staff" }
                });

            migrationBuilder.InsertData(
                table: "RoomAmenities",
                columns: new[] { "AmenityID", "AmenityName", "Description" },
                values: new object[,]
                {
                    { 1, "WiFi", "High-speed internet access" },
                    { 2, "TV", "Flat-screen television" }
                });

            migrationBuilder.InsertData(
                table: "RoomStatuses",
                columns: new[] { "StatusID", "StatusName" },
                values: new object[,]
                {
                    { 1, "Available" },
                    { 2, "Booked" },
                    { 3, "Occupied" }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "RoomID", "Price", "Rate", "RoomNumber", "RoomType", "ServiceID", "StatusID", "ThumnailRoom" },
                values: new object[,]
                {
                    { 1, null, 100m, 101, "Standard", 1, 1, "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill" },
                    { 2, null, 150m, 102, "Deluxe", 2, 1, "https://media.cnn.com/api/v1/images/stellar/prod/140127103345-peninsula-shanghai-deluxe-mock-up.jpg?q=w_2226,h_1449,x_0,y_0,c_fill" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Email", "EmailConfirmationToken", "FirstName", "IsEmailConfirmed", "LastName", "Password", "PhoneNumber", "ResetPasswordToken", "RoleID", "TokenCreatedAt", "UserName" },
                values: new object[,]
                {
                    { 1, "admin@example.com", "abc", "Admin", true, "User", "admin123", "123456789", null, 1, null, "admin" },
                    { 2, "john@example.com", "abc", "John", true, "Doe", "customer123", "987654321", null, 2, null, "customer1" },
                    { 3, "jane@example.com", "abc", "Jane", true, "Smith", "staff123", "111222333", null, 3, null, "staff1" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "BookingID", "CheckInDate", "CheckOutDate", "CustomerID", "PaymentMethodID", "ReportID", "RoomID", "TotalAmount" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 3, 22, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2852), new DateTime(2024, 3, 24, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2857), 2, 1, 1, 1, 200m },
                    { 2, new DateTime(2024, 3, 23, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2860), new DateTime(2024, 3, 25, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(2860), 3, 2, 2, 2, 300m }
                });

            migrationBuilder.InsertData(
                table: "EmployeeSchedules",
                columns: new[] { "ScheduleID", "EmployeeID", "ShiftEndTime", "ShiftStartTime" },
                values: new object[] { 1, 3, new DateTime(2024, 3, 21, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 21, 8, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "EventRooms",
                columns: new[] { "EventRoomID", "Amenities", "EventDate", "EventDescription", "EventName", "RoomID" },
                values: new object[] { 1, "Projector, Whiteboard", new DateTime(2024, 3, 28, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(3067), "Corporate conference", "Conference", 2 });

            migrationBuilder.InsertData(
                table: "HousekeepingTasks",
                columns: new[] { "TaskID", "RoomID", "ScheduledTime", "TaskDescription", "TaskStatus" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 3, 22, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(3076), "Clean room", "Pending" },
                    { 2, 2, new DateTime(2024, 3, 23, 14, 44, 3, 592, DateTimeKind.Local).AddTicks(3078), "Change beddings", "Pending" }
                });

            migrationBuilder.InsertData(
                table: "BookingDetails",
                columns: new[] { "BookingDetailID", "BookingID", "RoomID", "SpecialRequests" },
                values: new object[,]
                {
                    { 1, 1, 1, "Extra pillows" },
                    { 2, 2, 2, "Early check-in" }
                });

            migrationBuilder.InsertData(
                table: "Feedbacks",
                columns: new[] { "FeedbackID", "BookingID", "Comment", "CustomerID", "Rating" },
                values: new object[,]
                {
                    { 1, 1, "Great experience!", 2, 5 },
                    { 2, 2, "Could be better", 3, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingID",
                table: "BookingDetails",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomID",
                table: "BookingDetails",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistories_BookingID",
                table: "BookingHistories",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHistories_CustomerID",
                table: "BookingHistories",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PaymentMethodID",
                table: "Bookings",
                column: "PaymentMethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ReportID",
                table: "Bookings",
                column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomID",
                table: "Bookings",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_EmployeeID",
                table: "EmployeeSchedules",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_EventRooms_RoomID",
                table: "EventRooms",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingID",
                table: "Feedbacks",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_CustomerID",
                table: "Feedbacks",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_HousekeepingTasks_RoomID",
                table: "HousekeepingTasks",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserID",
                table: "PaymentMethods",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoomAmenities_AmenityID",
                table: "RoomRoomAmenities",
                column: "AmenityID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ServiceID",
                table: "Rooms",
                column: "ServiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_StatusID",
                table: "Rooms",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "BookingHistories");

            migrationBuilder.DropTable(
                name: "EmployeeSchedules");

            migrationBuilder.DropTable(
                name: "EventRooms");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "HousekeepingTasks");

            migrationBuilder.DropTable(
                name: "RoomRoomAmenities");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "RoomAmenities");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "HotelServices");

            migrationBuilder.DropTable(
                name: "RoomStatuses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
