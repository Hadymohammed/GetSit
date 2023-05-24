using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
