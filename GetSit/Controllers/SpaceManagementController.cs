    using GetSit.Common;
    using GetSit.Data;
    using GetSit.Data.enums;
    using GetSit.Data.Security;
    using GetSit.Data.Services;
    using GetSit.Data.ViewModels;

    using GetSit.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    namespace GetSit.Controllers
    {
        [Authorize(Roles = "Provider")]//Error:Convert UserRole to class
        public class SpaceManagementController : Controller
        {
            private readonly IWebHostEnvironment _env;

            #region Dependacies
            readonly IUserManager _userManager;
            readonly AppDBcontext _context;
            readonly ISpaceEmployeeService _providerService;
            readonly ISpaceService _spaceSerivce;
            readonly ISpaceHallService _hallService;
            readonly IHallFacilityService _hallFacilityService;
            readonly IHallPhotoService _hallPhotoService;
            readonly ISpaceService_Service _SpaceService_service;
            readonly IServicePhotoService _servicePhotoService;
            readonly IBookingService _bookingService;
            public SpaceManagementController(IUserManager userManager,
                AppDBcontext context,
                ISpaceEmployeeService spaceEmployeeService,
                ISpaceService spaceService,
                ISpaceHallService hallService,
                IHallFacilityService hallFacilityService,
                IHallPhotoService hallPhotoService,
                ISpaceService_Service SpaceService_service,
                IServicePhotoService servicePhotoService,
                IBookingService bookingService, IWebHostEnvironment env)
            {
                _env = env;
                _userManager = userManager;
                _context = context;
                _providerService = spaceEmployeeService;
                _spaceSerivce = spaceService;
                _hallService = hallService;
                _hallFacilityService = hallFacilityService;
                _hallPhotoService = hallPhotoService;
                _SpaceService_service = SpaceService_service;
                _servicePhotoService = servicePhotoService;
                _bookingService = bookingService;
            }
            #endregion

            public async Task<IActionResult> IndexAsync()
            {
                var SpaceIdStirng = "";
                var spaceIdInt = 0;
                if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
                {
                    var providerId = _userManager.GetCurrentUserId(HttpContext);
                    var provider = await _providerService.GetByIdAsync(providerId);
    #pragma warning disable CS8629 // Nullable value type may be null.
                    spaceIdInt = (int)provider.SpaceId;
    #pragma warning restore CS8629 // Nullable value type may be null.
                    SpaceIdStirng = provider.SpaceId.ToString();
                    if (SpaceIdStirng != String.Empty)
                        HttpContext.Response.Cookies.Append("SpaceId", SpaceIdStirng);
                }
                else
                {
                    SpaceIdStirng = HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value;
                    int.TryParse(SpaceIdStirng, out spaceIdInt);
                }
                Space space = await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos);
                SpaceManagementVM viewModel = new()
                {
                    Space = space,
                    Halls = _hallService.GetBySpaceId(spaceIdInt, h => h.HallPhotos, h => h.HallFacilities),
                    Services = _SpaceService_service.GetBySpaceId(spaceIdInt, s => s.ServicePhotos),
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
                var spaceIdInt = 0;
                if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
                {
                    var providerId = _userManager.GetCurrentUserId(HttpContext);
                    var provider = await _providerService.GetByIdAsync(providerId);
                    spaceIdInt = (int)provider.SpaceId;
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
                Space space = await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos);
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
                Space space = await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos);
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
                    Name = vm.SpaceName,
                    Description = vm.Description,
                    Price = vm.CostPerUnit
                };
                await _SpaceService_service.AddAsync(service);

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteHallPhoto(int PhotoId , int HallId)
        {
            string fileName = _hallPhotoService.GetPhotoFileName(PhotoId);
            string rootPath = _env.WebRootPath;
            string filePath = Path.Combine(rootPath, "photos", fileName);
            bool isDeleted = SaveFile.DeleteFile(filePath);
            if (isDeleted)
            {
                RedirectToAction("EditHall", HallId);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteServicePhoto(int PhotoId, int ServiceId)
        {
            string fileName = _servicePhotoService.GetPhotoFileName(PhotoId);
            string rootPath = _env.WebRootPath;
            string filePath = Path.Combine(rootPath, "photos", fileName);
            bool isDeleted = SaveFile.DeleteFile(filePath);
            if (isDeleted)
            {
                RedirectToAction("EditHall", ServiceId);
            }
            return NotFound();
        }
        #region edit hall
        [HttpGet]
    public async Task<IActionResult> EditHallAsync(int HallId)
        {
            if (HallId == 0)
                return RedirectToAction("Index");
            var hall = await _hallService.GetByIdAsync(HallId, h => h.HallPhotos, h => h.HallFacilities, h => h.Space);
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user.SpaceId != hall.SpaceId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var vm = new EditHallVM()
            {
                Id = hall.Id,
                Description = hall.Description,
                CostPerHour = hall.CostPerHour,
                Type = hall.Type,
                ThumbnailUrl = await _hallPhotoService.GetThumbnailUrlAsync(hall.Id),
            };
            return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> EditHallAsync(EditHallVM vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var hall = await _hallService.GetByIdAsync(vm.Id);
        hall.Description = vm.Description;
        hall.CostPerHour = vm.CostPerHour;
            int cnt = 1;
            foreach (var file in vm.Files)
            {
                var photo = new HallPhoto()
                {
                    HallId = hall.Id,
                    Url = "Temp",
                };
                    await _hallPhotoService.AddAsync(photo);
                    var filePath = SaveFile.HallPhoto(file, vm.SpaceName, hall.Id, photo.Id);
                if (filePath == null)
                {
                    await _hallPhotoService.DeleteAsync(photo.Id);
                }
                photo.Url = filePath.Result;
                await _hallPhotoService.UpdateAsync(photo.Id, photo);
            }
            await _hallService.UpdateAsync(hall.Id , hall);
        return RedirectToAction("Index");
        }
        public ISpaceService_Service Get_SpaceService_service()
        {
            return _SpaceService_service;
        }
        #endregion
        #region edit service 
        [HttpGet]
        public async Task<IActionResult> EditServiceAsync(int ServiceId)
        {
            if (ServiceId == 0)
                return RedirectToAction("Index");
            var service = await _hallService.GetByIdAsync(ServiceId);
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user.SpaceId != service.SpaceId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var vm = new AddServiceVM()
            {
                SpaceId = service.SpaceId,
                Description = service.Description,
                CostPerUnit = service.CostPerHour,
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditService(EditServiceVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var service = await _SpaceService_service.GetByIdAsync(vm.id);
            if (service == null)
            {
                return NotFound();
            }
            service.Name = vm.ServiceName;
            service.Description = vm.Description;
            service.Price = vm.CostPerUnit;
            int cnt = 1;
            foreach (var file in vm.Files)
            {
                var photo = new ServicePhoto()
                {
                    ServiceId = service.Id,
                    Url = "Temp",
                };
                await _servicePhotoService.AddAsync(photo);
                var filePath = SaveFile.HallPhoto(file, vm.SpaceName, service.Id, photo.Id);
                if (filePath == null)
                {
                    await _hallPhotoService.DeleteAsync(photo.Id);
                }
                photo.Url = filePath.Result;
                await _servicePhotoService.UpdateAsync(photo.Id, photo);
            }
            await _SpaceService_service.UpdateAsync(service.Id,service);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
