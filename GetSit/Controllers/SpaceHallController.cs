using GetSit.Data;
using GetSit.Data.Services;
using GetSit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GetSit.Controllers
{
    public class SpaceHallController : Controller
    {
        public readonly ISpaceHallService _service;
        public SpaceHallController(ISpaceHallService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index(string Key)
        {
            var data = await _service.GetAll();
            if (!String.IsNullOrEmpty(Key))
            {
                data = data.Where(p => p.Space.Name.Contains(Key));
            }
            return View(data);
        }
        public IActionResult Fav(int HId)
        {
            int CId = 1;
            _service.Fav(HId,CId);
            return View("Index");
        }
        //public ActionResult Search(string query)
        //{

        //    return View(results);
        //}
        //public ActionResult Searchh(string query)
        //{
        //    var searchResults = GetSearchResults(query);

        //    return View(searchResults);
        //}
        //[HttpPost]
        //private async Task<IActionResult> GetSearchResults(string Key)
        //{
        //    var data = await _service.GetBySearch(Key);
        //    return View(data);
            
        //}

    }
}
