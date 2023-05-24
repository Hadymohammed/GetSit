using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class ExploreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
