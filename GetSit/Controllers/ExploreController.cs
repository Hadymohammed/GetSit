
﻿using GetSit.Data;
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

       public readonly IExploreService _service;
        public ExploreController(IExploreService service)
        {
            _service = service;
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
        public IActionResult Fav(int HId)
        {
            int CId = 1;
            _service.Fav(HId,CId);
            return View("Index");
        }


    }
}
