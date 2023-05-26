using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.ViewModels;

using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Controllers
{
    [Authorize(Roles = "Provider")]//Convert UserRole to class
    public class SpaceManagementController : Controller
    {
        readonly IUserManager _userManager;
        readonly AppDBcontext _context;
        public SpaceManagementController(IUserManager userManager,AppDBcontext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var SpaceId="";
            if(HttpContext.Request.Cookies.Where(c=>c.Key=="SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = _context.SpaceEmployee.Where(e => e.Id == providerId).FirstOrDefault();
                SpaceId = provider.SpaceId.ToString();
                if(SpaceId != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceId);
            }
            else
            {
                SpaceId = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
            }
            Space space = _context.Space.Include(s=>s.Photos).Where(s => s.Id.ToString() == SpaceId).FirstOrDefault();
            SpaceManagementVM viewModel = new ()
            {
                Space=space,
                Halls = _context.SpaceHall.Include(h=>h.HallPhotos).Where(h=>h.SpaceId.ToString()==SpaceId).ToList(),
                Services= _context.SpaceService.Include(h => h.ServicePhotos).Where(h => h.SpaceId.ToString() == SpaceId).ToList(),
                Employees= _context.SpaceEmployee.Where(h => h.SpaceId.ToString() == SpaceId).ToList()
            };
            return View(viewModel);
        }
    }
}
