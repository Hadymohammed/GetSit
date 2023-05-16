using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
