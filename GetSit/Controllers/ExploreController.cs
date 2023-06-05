
﻿using GetSit.Data;
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

        private readonly IExploreService _service;
        private readonly IUserManager _userManager;
        public ExploreController(IExploreService service, IUserManager userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string Key)
        {
            var data = await _service.GetAll();
            if (!String.IsNullOrEmpty(Key))
            {
                data = data.Where(p => p.Space.Name.Contains(Key)|| p.Space.City.Contains(Key) || p.Space.Country.Contains(Key)|| p.Space.Street.Contains(Key));
            }
            return View(data);
        }
        [HttpGet]
        public IActionResult ToggleFavouriteHall(int hallId)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                int CustomerId = _userManager.GetCurrentUserId(HttpContext);
                _service.Fav(hallId, CustomerId);
            }
            return RedirectToAction("Index");
        }


    }
}
