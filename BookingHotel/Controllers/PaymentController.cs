using Microsoft.AspNetCore.Mvc;

namespace BookingHotel.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
