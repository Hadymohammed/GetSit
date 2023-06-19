using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data;
using Microsoft.AspNetCore.Mvc;
using GetSit.Data.enums;
using GetSit.Common;
using GetSit.Data.ViewModels;
using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SystemAdminController : Controller
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
        readonly IHallRequestService _hallRequestService;
        readonly ISystemAdminService _systemAdminService;
        readonly ICustomerService _customerService;
        public SystemAdminController(IUserManager userManager,
            AppDBcontext context,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService hallService,
            IHallFacilityService hallFacilityService,
            IHallPhotoService hallPhotoService,
            ISpaceService_Service spaceService_service,
            IServicePhotoService servicePhotoService,
            IBookingService bookingService,
            IHallRequestService HallRequestService,
            ISystemAdminService systemAdminService,
            ICustomerService customerService)

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
            _hallRequestService = HallRequestService;
            _systemAdminService = systemAdminService;
            _customerService = customerService;
        }
        bool PresirvedEmail(string email)
        {
            return (_customerService.GetByEmail(email) != null ||
                    _providerService.GetByEmail(email) != null ||
                    _systemAdminService.GetByEmail(email) != null
                    );
        }
        #endregion          
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
            #region Security
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user =await _providerService.GetByIdAsync(userId);
            #endregion
            List<Tuple<Space, SpaceEmployee>> Spaces = new List<Tuple<Space, SpaceEmployee>>();
            var DbSpaces = await _spaceSerivce.GetAllAsync();
			foreach (var space in DbSpaces)
			{
                if (space.IsApproved == false)
                {
                    var providers = _providerService.GetBySpaceId(space.Id);
					Spaces.Add(Tuple.Create(space, providers.FirstOrDefault()));

				}

			}
			var halls = _context.HallRequest.Where(i => i.Status == ReqestStatus.pending )
                .Include(r=>r.Hall)
                    .ThenInclude(h=>h.HallPhotos)
                /*.Include(r=>r.Hall)
                    .ThenInclude(h=>h.Space)*/
                .ToList();
            List<Tuple<HallRequest, Space>> requests = new List<Tuple<HallRequest, Space>>();
            foreach (var hall in halls)
            {
                var currentHall = _context.SpaceHall.Where(i=>i.Id == hall.Id).FirstOrDefault();
                var space = _context.Space.Where(i => i.Id == currentHall.SpaceId).FirstOrDefault();
                requests.Add(Tuple.Create(hall, space));
            }
            var viewModel = new SystemAdminVM
            {
                hallRequest = requests,
                Spaces = Spaces,
                NumberOfCustomers = _context.Customer.Count(),
                NumberOfSpaces = _context.Space.Count(),
                NumberOfGuestBookings = _context.GuestBooking.Count(),
				NumberOfBookings = _context.Booking.Count(),
		};

            return View(viewModel);
        }
        public IActionResult SpaceRequestDetails (int spaceId)
        {
            var space = _context.Space.Where(i=>i.Id==spaceId).FirstOrDefault();
			var provider = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault();
            var viewModel = new ReviewSpaceVM
            {
                SpaceId = space.Id,
                Space = space,
                spaceEmployee = provider,
            };
			return View(viewModel);
        }
		[HttpGet]
		public async Task<IActionResult> AcceptSpaceAsync(int SpaceId)
		{
            var space = await _spaceSerivce.GetByIdAsync(SpaceId, s => s.Employees);
			space.IsApproved=true;
            var JwtSecurtiyToken = JwtTokenHelper.GenerateJwtToken(space.Employees.First().Email, space.Employees.First().Id);
            var Token = new Token
            {
                token = JwtSecurtiyToken
            };
            await _context.Token.AddAsync(Token);
            _context.SaveChanges();
            var UId = Token.Id;
			await _spaceSerivce.UpdateAsync(space.Id,space); 
            string oneTimeAddStaffLink = Url.Action("RegisterProvider", "Account", new { UID = UId, Role = (int)UserRole.Provider, token = Token.token }, Request.Scheme);
            EmailHelper.SendEmailAddStaff(space.Employees.First().Email, oneTimeAddStaffLink, space.Name);

            return RedirectToAction("Index");
		}
		[HttpPost]
		public async Task<IActionResult> RejectSpace(RejectSpaceVM vm)
		{
            var space = await _spaceSerivce.GetByIdAsync(vm.SpaceId,s=>s.Employees);
            var spaceName = space.Name;
            var ManagerEmail = space.Employees.First().Email;
            var employeeIdsToDelete = space.Employees.Select(emp => emp.Id).ToList();

            foreach (var empId in employeeIdsToDelete)
            {
                await _providerService.DeleteAsync(empId);
            }
            await _spaceSerivce.DeleteAsync(space.Id);
            EmailHelper.SendSpaceRejection(ManagerEmail,vm.Messege,spaceName);
            
            return RedirectToAction("Index");
		}
		public async Task <IActionResult> HallRequest(int requestId)
        {
			var request =await _hallRequestService.GetByIdAsync(requestId);
			var hall =await _hallService.GetByIdAsync(request.HallId,h=>h.HallPhotos);
            var currentHall = _context.SpaceHall.Where(i=>i.Id == hall.Id).FirstOrDefault();
            var space = _context.Space.Where(i => i.Id == currentHall.SpaceId).FirstOrDefault();
			var provider = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault();
            var viewModel = new ReviewSpaceVM()
            {
                Space = space,
                spaceEmployee = provider,
                hallRequest = request,
                Hall = hall,
                RequestId=request.Id,
            };
			return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> AcceptHallReqestAsync(int requestId)
        {
            var request = await _hallRequestService.GetByIdAsync(requestId,r=>r.Hall);
            var hall = request.Hall;
            hall.Status = HallStatus.Accepted;
            await _hallService.UpdateAsync(hall.Id,hall);
            await _hallRequestService.DeleteAsync(request.Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> RejectHallRequestAsync(RejectHallVM vm)
        {
            var request = await _hallRequestService.GetByIdAsync(vm.RequestId,r=>r.Hall);
            request.Hall = await _hallService.GetByIdAsync(request.HallId, h => h.Space);
            var space = await _spaceSerivce.GetByIdAsync(request.Hall.SpaceId, s => s.Employees);
            request.comment = vm.Messege;
            request.Status = ReqestStatus.Rejected;
            request.Date = DateTime.Now;
            await _hallRequestService.UpdateAsync(request.Id,request);
            EmailHelper.SendHallRejection(space.Employees.First().Email, request.comment, space.Name);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddAdmin()
        {

            var UserId = _userManager.GetCurrentUserId(HttpContext);
            var User = await _systemAdminService.GetByIdAsync(UserId);
            //var Space = await _spaceSerivce.GetByIdAsync((int)User.SpaceId, s => s.Photos);
            return View(new SystemAdmin()
            {
                ProfilePictureUrl = User.ProfilePictureUrl,
            });
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(SystemAdmin NewAdmin)
        {
            
            ModelState.Remove("Password");
            if (!ModelState.IsValid)
            {
                return View(NewAdmin);
            }
            if (PresirvedEmail(NewAdmin.Email))
            {
                ModelState.AddModelError("Email", "This email already has an account.");
                return View(NewAdmin);
            }

            string password = RandomPassword.GenerateRandomPassword(8);
            // Using SpaceId as default value here
            var AddNewAdmin = new SystemAdmin()
            {
                FirstName = NewAdmin.FirstName,
                Birthdate = NewAdmin.Birthdate,
                LastName = NewAdmin.LastName,
                Email = NewAdmin.Email,
                Password = password,
                PhoneNumber = NewAdmin.PhoneNumber,
                Registerd = false
            };

            try
            {
                await _systemAdminService.AddAsync(AddNewAdmin);
                var AdminId = AddNewAdmin.Id;
                var JwtSecurtiyToken = Common.JwtTokenHelper.GenerateJwtToken(NewAdmin.Email, AdminId);
                var Token = new Token
                {
                    token = JwtSecurtiyToken
                };
                await _context.Token.AddAsync(Token);
                _context.SaveChanges();
                var UId = Token.Id;
                string oneTimeAddStaffLink = Url.Action("RegisterAdmin", "Account", new { UID = UId, Role = (int)UserRole.Admin, token = Token.token }, Request.Scheme);
                EmailHelper.SendEmailAddAdmin(NewAdmin.Email, oneTimeAddStaffLink);
                return RedirectToAction("Index");
            }
            catch (Exception error)
            {
                return View(NewAdmin);
            }
            return RedirectToAction("AddNewstaff");
        }


    }
}