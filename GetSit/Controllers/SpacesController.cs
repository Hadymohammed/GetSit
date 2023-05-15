using GetSit.Data;
using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class SpacesController : Controller
    {
        public readonly AppDBcontext _context;
        public SpacesController(AppDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Space.ToList();
            return View(data);
        }
    }
}
