using BookingHotel.Models;
using BookingHotel.Service;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HotelDbContext>(
    oprions => oprions.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"))
    );

builder.Services.AddScoped<EmailService>();

builder.Services.AddDistributedMemoryCache(); // Sử dụng bộ nhớ cache để lưu trữ session, có thể thay đổi tùy thuộc vào nhu cầu.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian không hoạt động tối đa trước khi session hết hiệu lực (30 phút).
    options.IOTimeout = TimeSpan.FromSeconds(10); // Thời gian tối đa cho các hoạt động I/O của session (10 giây).
    options.Cookie.HttpOnly = true; // Cookie chỉ có thể được truy cập thông qua HTTP.
    options.Cookie.IsEssential = true; // Cookie là một phần thiết yếu của session.
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always; // Yêu cầu HTTPS để gửi cookie.
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict; // Đảm bảo cookie chỉ được gửi khi trang web được gọi từ trang web của chính mình.
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<VNpayService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
