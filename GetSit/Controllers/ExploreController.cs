
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

       readonly IExploreService _service;
        readonly UserManager _userManager;
        public ExploreController(IExploreService service,UserManager userManager)
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
        public IActionResult AddToFavorite(int HId)
        {
            int CId = _userManager.GetCurrentUserId(HttpContext);
            
            _service.Fav(HId,CId);
            return View("Index");
        }


    }
}
