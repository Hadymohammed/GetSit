using GetSit.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class SpacesController : Controller
    {
        public readonly ISpaceService _service;
        public SpacesController(ISpaceService service)
        {
            _service = service;
        }
        public async Task<IActionResult >Index()
        {
            var data =await _service.GetAll();
            return View(data);
        }
    }
}
