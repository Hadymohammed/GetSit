using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data.ViewModels;
using System.Web;
using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;

namespace GetSit.Controllers
{
    [Authorize(Roles = "Provider")]//Error:Convert UserRole to class
    public class SpaceManagementController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ISpaceEmployeeService _spaceEmployeeService;
        private readonly ISystemAdminService _adminSerivce;
        private readonly ISpacePhoneService _spacePhoneService;
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
            IBookingService bookingService, 
            ISpacePhoneService spacePhoneService)
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
            Space space = await _spaceSerivce.GetByIdAsync(spaceIdInt, s => s.Photos);
            SpaceManagementVM viewModel = new()
            {
                Space = space,
                Halls = _hallService.GetBySpaceId(spaceIdInt, h => h.HallPhotos, h => h.HallFacilities),
                Services = _spaceService_service.GetBySpaceId(spaceIdInt, s => s.ServicePhotos),
                Employees = _providerService.GetBySpaceId(spaceIdInt),
                Bookings = _bookingService.GetBySpaceId(spaceIdInt)
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult AddPhoneNumber()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPhoneNumber(SpacePhone AddNewSpacePhone)
        {
            if (!ModelState.IsValid)
            {
                return View(AddNewSpacePhone);
            }
            HttpContext.Session.SetString("NewNumberModel", JsonConvert.SerializeObject(AddNewSpacePhone));
            var otpVm = new OTPVM();
            otpVm.Phone = AddNewSpacePhone.PhoneNumber;
            return RedirectToAction("PhoneOTP", otpVm);
        }
        [HttpGet]
        public IActionResult PhoneOTP(OTPVM? otpVm)
        {
            if (otpVm is null)
                RedirectToAction("AddPhoneNumber");
            OTPServices.SendPhoneOTP(HttpContext, otpVm.Phone);
            return View(otpVm);
        }
        [HttpPost]
        public async Task<IActionResult> PhoneOTPAsync(OTPVM otp)
        {
            if (!ModelState.IsValid)
            {
                return View(otp);
            }
            if (OTPServices.VerifyOTP(HttpContext, otp) == false)
            {
                ModelState.AddModelError("OTP", "InValid Code");
                return View(otp);
            }
            var stringUser = HttpContext.Session.GetString("NewNumberModel");
            var AddPhoneNumber = JsonConvert.DeserializeObject<RegisterVM>(stringUser) as RegisterVM;
            if (AddPhoneNumber is null)
                RedirectToAction("AddPhoneNumber");
            // Using SpaceId as default value here
            var SpaceId = 1;
            var NewPhone = new SpacePhone()
            {
                PhoneNumber = AddPhoneNumber.PhoneNumber,
                SpaceId = SpaceId,
            };

            try
            {
                await _spacePhoneService.AddAsync(NewPhone);
                return RedirectToAction("SpaceManagement");
            }
            catch (Exception error)
            {
                return View(AddPhoneNumber);
            }
            return RedirectToAction("AddPhoneNumber");
        }
        [HttpGet]
        public IActionResult AddContact()
        {
            return View();
        }
        [HttpPost]

        #region Create New Hall
        [HttpGet]
        public async Task<IActionResult> AddHallAsync()
        {
            var SpaceIdStirng = "";
            var spaceIdInt = 0;
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
        [HttpGet]
        public IActionResult AddStaff()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddStaff(SpaceEmployee NewStaff)
        {
            if (!ModelState.IsValid)
            {
                return View(NewStaff);
            }
            string password = RandomPassword.GenerateRandomPassword(8);
            // Using SpaceId as default value here
            var AddNewstaff = new SpaceEmployee()
            {
                FirstName = NewStaff.FirstName,
                LastName = NewStaff.LastName,
                Email = NewStaff.Email,
                Password = PasswordHashing.Encode(password),
                PhoneNumber = NewStaff.PhoneNumber,
                Birthdate = NewStaff.Birthdate,
                EmployeeRole = NewStaff.EmployeeRole
            };

            try
            {
                await _spaceEmployeeService.AddAsync(AddNewstaff);
                var SpaceEmployeeId = AddNewstaff.Id;
                var JwtSecurtiyToken = Common.JwtTokenHelper.GenerateJwtToken(NewStaff.Email, SpaceEmployeeId);
                var Token = new Token
                {
                    token = JwtSecurtiyToken
                };
                _context.Token.AddAsync(Token);
                var UId = Token.Id;
                string oneTimeAddStaffLink = Url.Action("RegisterProvider", "Account", new { uid = UId, token = Token }, Request.Scheme);
                Common.AddStaffEmail.SendEmailAddStaff(NewStaff.Email, oneTimeAddStaffLink);
                return RedirectToAction("SpaceManagement");
            }
            catch (Exception error)
            {
                return View(NewStaff);
            }
            return RedirectToAction("AddNewstaff");
        }

        #endregion
    }
}
