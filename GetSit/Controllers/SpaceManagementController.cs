using GetSit.Common;
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
    [Authorize(Roles = "Provider")]//Error:Convert UserRole to class
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
        #region CreateNewHAll
        [HttpGet]
        public IActionResult AddHall()
        {
            var SpaceId = "";
            if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = _context.SpaceEmployee.Where(e => e.Id == providerId).FirstOrDefault();
                SpaceId = provider.SpaceId.ToString();
                if (SpaceId != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceId);
            }
            else
            {
                SpaceId = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
            }
            Space space = _context.Space.Include(s => s.Photos).Where(s => s.Id.ToString() == SpaceId).FirstOrDefault();
            AddHallVM vm = new()
            {
                SpaceId = space.Id,
                SpaceName = space.Name,
                SpaceBio = space.Bio,
                SpacePhotoUrl = space.Photos.First().Url
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddHall(AddHallVM vm, Facility[] Facilities)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            /*Add new hall to the database*/
            var hall = new SpaceHall()
            {
                SpaceId = vm.SpaceId,
                Description = vm.Description,
                CostPerHour = vm.CostPerHour,
                Capacity = vm.Capacity,
                Type = vm.Type,
                Status = HallStatus.Accepted
            };
            await _context.SpaceHall.AddAsync(hall);
            await _context.SaveChangesAsync();
            /*Add Facilities*/
            foreach (Facility facility in Facilities)
            {
                await _context.HallFacility.AddAsync(new HallFacility()
                {
                    Facility = facility,
                    HallId = hall.Id
                });
                await _context.SaveChangesAsync();
            }
            /*Add thumbnail*/
            int cnt = 0;
            var thumbnailPath = SaveFile.HallPhoto(vm.Thumbnail, vm.SpaceName, hall.Id, cnt);
            if (thumbnailPath != null)
            {
                var thumbnail = new HallPhoto()
                {
                    HallId = hall.Id,
                    Url = thumbnailPath.Result,
                };

                await _context.HallPhoto.AddAsync(thumbnail);
                await _context.SaveChangesAsync();
            }
            else return View(vm);
            /*Add hall Photos*/
            foreach (var file in vm.Files)
            {
                cnt++;
                var filePath = SaveFile.HallPhoto(file, vm.SpaceName, hall.Id, cnt);
                if (filePath != null)
                {
                    /*Add hall photo*/
                    var photo = new HallPhoto()
                    {
                        HallId = hall.Id,
                        Url = filePath.Result,
                    };
                    await _context.HallPhoto.AddAsync(photo);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region CreateNewService
        [HttpGet]
        public IActionResult AddService()
        {
            var SpaceId = "";
            if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = _context.SpaceEmployee.Where(e => e.Id == providerId).FirstOrDefault();
                SpaceId = provider.SpaceId.ToString();
                if (SpaceId != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceId);
            }
            else
            {
                SpaceId = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
            }
            Space space = _context.Space.Include(s => s.Photos).Where(s => s.Id.ToString() == SpaceId).FirstOrDefault();
            AddServiceVM vm = new()
            {
                SpaceId = space.Id,
                SpaceName = space.Name,
                SpaceBio = space.Bio,
                SpacePhotoUrl = space.Photos.First().Url
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddService(AddServiceVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            /*Add new Service to the database*/
            var service = new SpaceService()
            {
                SpaceId = vm.SpaceId,
                Name=vm.SpaceName,
                Description = vm.Description,
                Price = vm.CostPerUnit
            };
            await _context.SpaceService.AddAsync(service);
            await _context.SaveChangesAsync();
            
            /*Add thumbnail*/
            int cnt = 0;
            var thumbnailPath = SaveFile.HallPhoto(vm.Thumbnail, vm.SpaceName, service.Id, cnt);
            if (thumbnailPath != null)
            {
                var thumbnail = new ServicePhoto()
                {
                    ServiceId = service.Id,
                    Url = thumbnailPath.Result,
                };

                await _context.ServicePhoto.AddAsync(thumbnail);
                await _context.SaveChangesAsync();
            }
            else return View(vm);
            /*Add hall Photos*/
            foreach (var file in vm.Files)
            {
                cnt++;
                var filePath = SaveFile.HallPhoto(file, vm.SpaceName, service.Id, cnt);
                if (filePath != null)
                {
                    /*Add hall photo*/
                    var photo = new ServicePhoto()
                    {
                        ServiceId = service.Id,
                        Url = filePath.Result,
                    };
                    await _context.ServicePhoto.AddAsync(photo);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
