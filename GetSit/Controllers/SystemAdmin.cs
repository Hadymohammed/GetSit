using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class SystemAdmin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
