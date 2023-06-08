using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data.ViewModels;

using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GetSit.Controllers
{
    [Authorize(Roles = "Provider")]//Error:Convert UserRole to class
    public class SpaceManagementController : Controller
    {
        #region Dependacies
        readonly IUserManager _userManager;
        readonly AppDBcontext _context;
        readonly ISpaceEmployeeService _providerService;
        readonly ISpaceService _spaceSerivce;
        readonly ISpaceHallService _hallService;
        readonly IHallFacilityService _hallFacilityService;
        readonly IHallPhotoService _hallPhotoService;
        readonly ISpaceService_Service _spaceService_service;
        readonly IServicePhotoService _servicePhotoService;
        readonly IBookingService _bookingService;
        public SpaceManagementController(IUserManager userManager,
            AppDBcontext context,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService hallService,
            IHallFacilityService hallFacilityService,
            IHallPhotoService hallPhotoService,
            ISpaceService_Service spaceService_service,
            IServicePhotoService servicePhotoService,
            IBookingService bookingService)
        {
            _userManager = userManager;
            _context = context;
            _providerService = spaceEmployeeService;
            _spaceSerivce = spaceService;
            _hallService = hallService;
            _hallFacilityService = hallFacilityService;
            _hallPhotoService = hallPhotoService;
            _spaceService_service = spaceService_service;
            _servicePhotoService = servicePhotoService;
            _bookingService = bookingService;
        }
        #endregion
        public async Task<IActionResult> IndexAsync()
        {
            var SpaceIdStirng = "";
            var spaceIdInt = 0;
            if (HttpContext.Request.Cookies.Where(c=>c.Key=="SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = await _providerService.GetByIdAsync(providerId);
                spaceIdInt = (int)provider.SpaceId;
                SpaceIdStirng = spaceIdInt.ToString();
                if(SpaceIdStirng != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceIdStirng);
            }
            else
            {
                SpaceIdStirng = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
                int.TryParse(SpaceIdStirng, out spaceIdInt);
            }
            Space space =await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos);
            SpaceManagementVM viewModel = new()
            {
                Space = space,
                Halls = _hallService.GetBySpaceId(spaceIdInt,h=>h.HallPhotos,h=>h.HallFacilities),
                Services = _spaceService_service.GetBySpaceId(spaceIdInt, s => s.ServicePhotos),
                Employees = _providerService.GetBySpaceId(spaceIdInt),
                Bookings = _bookingService.GetBySpaceId(spaceIdInt)
        };
            return View(viewModel);
        }
        #region Create New Hall
        [HttpGet]
        public async Task<IActionResult> AddHallAsync()
        {
            var SpaceIdStirng = "";
            var spaceIdInt=0;
            if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                //var provider = _context.SpaceEmployee.Where(e => e.Id == providerId).FirstOrDefault();
                var provider = await _providerService.GetByIdAsync(providerId);
                SpaceIdStirng = provider.SpaceId.ToString();
                if (SpaceIdStirng != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceIdStirng);
            }
            else
            {
                SpaceIdStirng = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
                int.TryParse(SpaceIdStirng, out spaceIdInt);
            }
            //Space space = _context.Space.Include(s => s.Photos).Where(s => s.Id.ToString() == SpaceId).FirstOrDefault();
            Space space =await _spaceSerivce.GetByIdAsync(spaceIdInt, s=>s.Photos);
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
            await _hallService.AddAsync(hall);
            /*Add Facilities*/
            foreach (Facility facility in Facilities)
            {
                await _hallFacilityService.AddAsync(new HallFacility()
                {
                    Facility = facility,
                    HallId = hall.Id
                });
            }
            /*Add thumbnail*/
            int cnt = 0;
            var thumbnailPath = SaveFile.HallPhoto(vm.Thumbnail, vm.SpaceName, hall.Id, cnt);
            if (thumbnailPath != null)
            {
                await _hallPhotoService.AddAsync(new HallPhoto()
                {
                    HallId = hall.Id,
                    Url = thumbnailPath.Result,
                });
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
                    await _hallPhotoService.AddAsync(new HallPhoto()
                    {
                        HallId = hall.Id,
                        Url = filePath.Result,
                    });
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Create New Service
        [HttpGet]
        public async Task<IActionResult> AddServiceAsync()
        {
            var SpaceIdStirng = "";
            var spaceIdInt = 0;
            if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
            {
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = _context.SpaceEmployee.Where(e => e.Id == providerId).FirstOrDefault();
                spaceIdInt = (int)provider.SpaceId;
                SpaceIdStirng = spaceIdInt.ToString();
                if (SpaceIdStirng != String.Empty)
                    HttpContext.Response.Cookies.Append("SpaceId", SpaceIdStirng);
            }
            else
            {
                SpaceIdStirng = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
                int.TryParse(SpaceIdStirng, out spaceIdInt);
            }
            //Space space = _context.Space.Include(s => s.Photos).Where(s => s.Id.ToString() == SpaceId).FirstOrDefault();
            Space space =await _spaceSerivce.GetByIdAsync(spaceIdInt, s=>s.Photos);
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
                Name=vm.ServiceName,
                Description = vm.Description,
                Price = vm.CostPerUnit
            };
            await _spaceService_service.AddAsync(service);
            
            /*Add thumbnail*/
            int cnt = 0;
            var thumbnailPath = SaveFile.HallPhoto(vm.Thumbnail, vm.SpaceName, service.Id, cnt);
            if (thumbnailPath != null)
            {
                await _servicePhotoService.AddAsync(new ServicePhoto()
                {
                    ServiceId = service.Id,
                    Url = thumbnailPath.Result,
                });
            }
            else return View(vm);
            /*Add Service Photos*/
            foreach (var file in vm.Files)
            {
                cnt++;
                var filePath = SaveFile.ServicePhoto(file, vm.SpaceName, service.Id, cnt);
                if (filePath != null)
                {
                    /*Add hall photo*/
                    
                    await _servicePhotoService.AddAsync(new ServicePhoto()
                    {
                        ServiceId = service.Id,
                        Url = filePath.Result,
                    });
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
