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
        readonly ISpacePhoneService _spacePhoneService;
        readonly ISpacePhotoService _spacePhotoService;
        public SpaceManagementController(IUserManager userManager,
            AppDBcontext context,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService hallService,
            IHallFacilityService hallFacilityService,
            IHallPhotoService hallPhotoService,
            ISpaceService_Service spaceService_service,
            IServicePhotoService servicePhotoService,
            IBookingService bookingService,
            ISpacePhoneService spacePhoneService,
            ISpacePhotoService spacePhotoService)
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
            _spacePhoneService = spacePhoneService;
            _spacePhotoService = spacePhotoService;
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
            Space space =await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos,s=>s.Phones);
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
        #region SpaceDetails
        public async Task<IActionResult> SpaceDetails(int SpaceId)
        {
            if (SpaceId == 0)
                return NotFound();
            int userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user == null)
                return RedirectToAction("AccessDenied", "Account");
            if(user.SpaceId!=SpaceId)
                return RedirectToAction("AccessDenied", "Account");

            var space = await _spaceSerivce.GetByIdAsync((int)user.SpaceId,s=>s.Photos,s=>s.Phones);
            return View(new SpaceDetailsVM()
            {
                Space=space
            });
        }
        [HttpPost]
        public async Task<IActionResult> SpaceDetails(SpaceDetailsVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Space= await _spaceSerivce.GetByIdAsync(vm.Space.Id, s => s.Photos, s => s.Phones);
                return View(vm);
            }
            var space = await _spaceSerivce.GetByIdAsync(vm.Space.Id);
            int userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user == null)
                return RedirectToAction("AccessDenied", "Account");
            if (user.SpaceId != space.Id)
                return RedirectToAction("AccessDenied", "Account");

            space.Bio = vm.Space.Bio;
            space.Facebook = vm.Space.Facebook;
            space.Twitter = vm.Space.Twitter;
            space.Instagram = vm.Space.Instagram;
            space.Email = vm.Space.Email;
            space.Country = vm.Space.Country;
            space.City=vm.Space.City;
            space.Street = vm.Space.Street;

            /*save phones*/
            if (vm.NewPhones != null)
            {
                foreach (var phone in vm.NewPhones)
                {
                    var nwPhone = new SpacePhone()
                    {
                        SpaceId = space.Id,
                        PhoneNumber = phone
                    };
                    await _spacePhoneService.AddAsync(nwPhone);
                }
            }
            /*Update Logo*/
            if (vm.Logo != null)
            {
                /*delete old one*/
                var oldPath = space.SpaceLogo;

                if ((oldPath != null && SaveFile.DeleteFile(oldPath))|| oldPath == null)
                {
                    string nwPath = await SaveFile.SpaceLogo(vm.Logo, space.Name);
                    if (nwPath != null)
                    {
                        space.SpaceLogo = nwPath;
                    }

                    else space.SpaceLogo = "resource/site/logo-social.png";
                }
                
            }
            /*Update Cover*/
            if (vm.Cover != null)
            {
                /*delete old one*/
                var oldPath = space.SpaceCover;

                if ((oldPath != null && SaveFile.DeleteFile(oldPath))|| oldPath == null)
                {
                    string nwPath = await SaveFile.SpaceCover(vm.Cover, space.Name);
                    if (nwPath != null)
                    {
                        space.SpaceCover = nwPath;
                    }

                    else space.SpaceCover = "resource/site/Cover_PlaceHolder.png";
                }

            }
            await _spaceSerivce.UpdateAsync(space.Id, space);
            return RedirectToAction("SpaceDetails", new { SpaceId = space.Id });
        }
        [HttpGet]
        public async Task<IActionResult> DeletePhone(int PhoneId)
        {
            if (PhoneId == 0)
                return NotFound();
            int userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            if (user == null)
                return RedirectToAction("AccessDenied", "Account");
            var phone =await _spacePhoneService.GetByIdAsync(PhoneId);
            if (phone == null)
                return NotFound();

            if (user.SpaceId != phone.SpaceId)
                return RedirectToAction("AccessDenied", "Account");
            await _spacePhoneService.DeleteAsync(phone.Id);
            return RedirectToAction("SpaceDetails", new { SpaceId = phone.SpaceId });
        }
        #endregion

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
