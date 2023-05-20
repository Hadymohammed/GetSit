using GetSit.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class SpaceHallController : Controller
    {
        public readonly ISpaceHallService _service;
        public SpaceHallController(ISpaceHallService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAll();
            return View(data);
        }
        public IActionResult Favrite(int id,int userID) {
        
        
        }
    }
}
