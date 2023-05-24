using GetSit.Data;
using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class BookingController : Controller
    {
        AppDBcontext _context;
        public BookingController( AppDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index(int HallID)
        {
            return View();
        }
        [HttpGet]
        public IActionResult HallBookingDetails(int HallId)
        {
            var Hall = _context.SpaceHall.Where(h => h.Id == HallId).FirstOrDefault();
            return View(HallId);
        }
    }
}
