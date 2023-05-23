using GetSit.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Controllers
{
    public class ExploreController : Controller
    {
        public raedonly ExploreController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string query)
        {
           
            return View(results);
        }
        public ActionResult Searchh(string query)
        {
            var searchResults = GetSearchResults(query);

            return View(searchResults);
        }
        private List<Space> GetSearchResults(string query)
        {
            
            var Spaces = AppDBContext.Spaces.Where(p => p.Name.Contains(query)).ToList();

            return Spaces;
        }

    }

}
