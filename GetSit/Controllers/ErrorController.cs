using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/NotFound")]
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }
    }
}
