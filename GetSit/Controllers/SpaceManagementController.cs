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
        readonly ISpaceService_Service _spaceService_service;
        readonly IServicePhotoService _servicePhotoService;
        readonly IBookingService _bookingService;

        readonly IHallRequestService _hallRequestService;

        readonly ISpacePhotoService _spacePhotoService;


        public SpaceManagementController(IUserManager userManager,
            AppDBcontext context,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService hallService,
            IHallFacilityService hallFacilityService,
            IHallPhotoService hallPhotoService,
            ISpaceService_Service SpaceService_service,
            IServicePhotoService servicePhotoService,
            IBookingService bookingService,

            IHallRequestService HallRequestService,

          ISpacePhotoService spacePhotoService,
            IWebHostEnvironment env)

        {
            _env = env;
            _userManager = userManager;
            _context = context;
            _providerService = spaceEmployeeService;
            _spaceSerivce = spaceService;
            _hallService = hallService;
            _hallFacilityService = hallFacilityService;
            _hallPhotoService = hallPhotoService;
            _spaceService_service = SpaceService_service;
            _servicePhotoService = servicePhotoService;
            _bookingService = bookingService;

            _hallRequestService = HallRequestService;

            _spacePhotoService = spacePhotoService;

        }
        #endregion

        public async Task<IActionResult> IndexAsync()
        {
            var SpaceIdStirng = "";
            var spaceIdInt = 0;
            if (HttpContext.Request.Cookies.Where(c => c.Key == "SpaceId").FirstOrDefault().Value is null)
            {
                /*replace spaceId cookie (Security)*/
                var providerId = _userManager.GetCurrentUserId(HttpContext);
                var provider = await _providerService.GetByIdAsync(providerId);

                spaceIdInt = (int)provider.SpaceId;

                SpaceIdStirng = provider.ToString();

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

                Halls = _hallService.GetAcceptedBySpaceId(spaceIdInt, h => h.HallPhotos, h => h.HallFacilities),
                Services = _spaceService_service.GetBySpaceId(spaceIdInt, s => s.ServicePhotos),
                Employees = _providerService.GetBySpaceId(spaceIdInt),
                Bookings = _bookingService.GetBySpaceId(spaceIdInt),
                Requests = _hallRequestService.GetPendingBySpaceId(spaceIdInt),

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
            AddHallVM vm = new AddHallVM()

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
            /*Add request*/
            var request = new HallRequest()
            {
                Hall = hall,
                Status = ReqestStatus.pending,
                Date = DateTime.Now,
            };
            _hallRequestService.AddRequest(request);

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
                Name = vm.ServiceName,
                Description = vm.Description,
                Price = vm.CostPerUnit
            };
            await _spaceService_service.AddAsync(service);

            /*Add thumbnail*/
            int cnt = 0;
            var thumbnailPath = SaveFile.ServicePhoto(vm.Thumbnail, vm.SpaceName, service.Id, cnt);
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

        #region Request Deteles
        public async Task<IActionResult> RequestDeteles(int RequestId)
        {

            var request = _hallRequestService.GetById(RequestId);
            return View(request);
        }

        #endregion

        
        #region Edit Hall
        [HttpGet]
        public async Task<IActionResult> EditHall(int HallId)
        {
            if (HallId == 0)
                return RedirectToAction("Index");
            var hall = await _hallService.GetByIdAsync(HallId, h => h.HallPhotos, h => h.HallFacilities, h => h.Space);
            if (hall == null)
                return RedirectToAction("Index");
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user.SpaceId != hall.SpaceId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var space = await _spaceSerivce.GetByIdAsync((int)user.SpaceId,s=>s.Photos);
            var vm = new EditHallVM()
            {
                SpaceId = space.Id,
                SpaceBio = space.Bio,
                SpaceName = space.Name,
                SpacePhotoUrl = space.Photos.First().Url,
                Description = hall.Description,
                CostPerHour = hall.CostPerHour,
                Type = hall.Type,
                Capacity = hall.Capacity,
                HallPhotos = hall.HallPhotos,
                HallFacilities=hall.HallFacilities
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditHallAsync(EditHallVM vm, Facility[] facilities)
        {
            if (!ModelState.IsValid)
            {
                vm.HallPhotos = _hallPhotoService.GetByHallId(vm.HallId);
                vm.HallFacilities= _hallFacilityService.GetByHallId(vm.HallId);
                return View(vm);
            }

            var hall = await _hallService.GetByIdAsync(vm.HallId);
            hall.Description = vm.Description;
            hall.CostPerHour = vm.CostPerHour;
            //Add new Photos
            foreach (var file in vm.Files)
            {
                var photo = new HallPhoto()
                {
                    HallId = hall.Id,
                    Url = "Temp",
                };
                await _hallPhotoService.AddAsync(photo);
                var filePath = await SaveFile.HallPhoto(file, vm.SpaceName, hall.Id, photo.Id);
                if (filePath == null)
                {
                    //undo adding photo
                    await _hallPhotoService.DeleteAsync(photo.Id);
                }
                else
                {
                    photo.Url = filePath;
                    await _hallPhotoService.UpdateAsync(photo.Id, photo);
                }
            }
           
            /*!!!!! Toggele Facilities change*/
            
            await _hallService.UpdateAsync(hall.Id, hall);
            return RedirectToAction("EditHall", new { hallId = hall.Id });
        }
        [HttpGet]
        public async Task<ActionResult> DeleteHallPhoto(int PhotoId)
        {
            var userId =_userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            
            var photo = await _hallPhotoService.GetByIdAsync(PhotoId);
            var hallId = photo.HallId;
            var hall = await _hallService.GetByIdAsync(hallId,h=>h.HallPhotos);

            if(user==null)
                return RedirectToAction("Index");
            if (user.SpaceId != hall.SpaceId)
                return RedirectToAction("AccessDenied", "Account");
                return NotFound();
            if (hall.HallPhotos.Count() <= 1)
            {
                return NotFound();
            }
            string fileName = photo.Url;
            string rootPath = _env.WebRootPath;
            string filePath = Path.Combine(rootPath,fileName);
            bool isDeleted = SaveFile.DeleteFile(filePath);
            if (isDeleted)
            {
                await _hallPhotoService.DeleteAsync(photo.Id);
                return RedirectToAction("EditHall", new {hallId=hall.Id});
            }
            return NotFound();
        }
        #endregion

        #region edit service 
        [HttpGet]
        public async Task<IActionResult> EditServiceAsync(int ServiceId)
        {
            if (ServiceId == 0)
                return NotFound();

            var service = await _spaceService_service.GetByIdAsync(ServiceId,s=>s.ServicePhotos);
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user.SpaceId != service.SpaceId)
            {
                return RedirectToAction("AccessDenied", "Account");
            }
            var space = await _spaceSerivce.GetByIdAsync(service.SpaceId,s=>s.Photos);
            var vm = new EditServiceVM()
            {
                SpaceId = space.Id,
                SpaceBio = space.Bio,
                SpaceName = space.Name,
                SpacePhotoUrl = space.Photos.First().Url,
                ServiceId=service.Id,
                ServiceName = service.Name,
                Description=service.Description,
                Price=service.Price,
                ServicePhotos=_servicePhotoService.GetByServiceId(service.Id)
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> EditService(EditServiceVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.ServicePhotos= _servicePhotoService.GetByServiceId(vm.ServiceId);
                return View(vm);
            }
            var service = await _spaceService_service.GetByIdAsync(vm.ServiceId);
            if (service == null)
            {
                return NotFound();
            }

            service.Name = vm.ServiceName;
            service.Description = vm.Description;
            service.Price = vm.Price;

            foreach (var file in vm.Files)
            {
                var photo = new ServicePhoto()
                {
                    ServiceId = service.Id,
                    Url = "Temp",
                };
                await _servicePhotoService.AddAsync(photo);
                var filePath = SaveFile.ServicePhoto(file, vm.SpaceName, service.Id, photo.Id);
                if (filePath == null)
                {
                    await _hallPhotoService.DeleteAsync(photo.Id);
                }
                else
                {
                    //Undo changes
                    photo.Url = filePath.Result;
                    await _servicePhotoService.UpdateAsync(photo.Id, photo);
                }
            }
            await _spaceService_service.UpdateAsync(service.Id, service);
            return RedirectToAction("EditService", new { ServiceId = service.Id });
        }
        [HttpGet]
        public async Task<ActionResult> DeleteServicePhoto(int PhotoId)
        {
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);

            var photo = await _servicePhotoService.GetByIdAsync(PhotoId);
            var serviceId = photo.ServiceId;
            var service = await _spaceService_service.GetByIdAsync(serviceId,s=>s.ServicePhotos);

            if (user == null)
                return RedirectToAction("AccessDenied", "Account");
            if (user.SpaceId != service.SpaceId)
                return RedirectToAction("AccessDenied", "Account");
            if (service.ServicePhotos.Count() <= 1)
            {
                return NotFound();
            }

            string fileName = photo.Url;
            string rootPath = _env.WebRootPath;
            string filePath = Path.Combine(rootPath, fileName);
            bool isDeleted = SaveFile.DeleteFile(filePath);
            if (isDeleted)
            {
                await _servicePhotoService.DeleteAsync(photo.Id);
                return RedirectToAction("EditService", new { ServiceId = service.Id });
            }
            return NotFound();
        }
        #endregion

    }
}