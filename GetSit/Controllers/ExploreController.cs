using GetSit.Data;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GetSit.Controllers
{
    public class ExploreController : Controller
    {

        private readonly IUserManager _userManager;
        private readonly ISpaceHallService _hallService;
        private readonly IFavoriteHallService _favoriteService;
        public ExploreController(IUserManager userManager, ISpaceHallService hallService, IFavoriteHallService favoriteService)
        {
            _userManager = userManager;
            _hallService = hallService;
            _favoriteService = favoriteService;
        }
        public async Task<IActionResult> Index(string Key)
        {
            var data = await _hallService.GetAllAsync(h => h.HallPhotos, h => h.Space, h => h.FavoriteHalls);
            if (!String.IsNullOrEmpty(Key))
            {
                data = data.Where(p => p.Space.Name.Contains(Key)|| p.Space.City.Contains(Key) || p.Space.Country.Contains(Key)|| p.Space.Street.Contains(Key));
            }
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> ToggleFavouriteHallAsync(int hallId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int CustomerId = _userManager.GetCurrentUserId(HttpContext);
                await _favoriteService.ToggleAysnc(hallId, CustomerId);
            }
            return RedirectToAction("Index");
        }


    }
}
