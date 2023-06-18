using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data;
using Microsoft.AspNetCore.Mvc;
using GetSit.Data.enums;
using GetSit.Common;
using GetSit.Data.ViewModels;
using GetSit.Models;

namespace GetSit.Controllers
{
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
            IHallRequestService HallRequestService)

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
        }
        #endregion          
        [HttpGet]
        public async Task<IActionResult> Index() 
        {
			List<Tuple<Space, SpaceEmployee>> Spaces = new List<Tuple<Space, SpaceEmployee>>();
			foreach (var space in _context.Space)
			{
                if (space.IsApproved == false)
                {
                    var provider = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault();
					Spaces.Add(Tuple.Create(space, provider));

				}

			}
			var halls = _context.HallRequest.Where(i => i.Status == 0 ).ToList();
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
        public IActionResult ViewSpaceRequest (int spaceId)
        {
            var space = _context.Space.Where(i=>i.Id==spaceId).FirstOrDefault();
			var provider = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault();
            var viewModel = new ReviewSpaceVM
            {
                Space = space,
                spaceEmployee = provider,
            };
			return View(viewModel);
        }
		[HttpGet]
		public IActionResult AcceptSpace(int SpaceId)
		{
			var space = _context.Space.Where(i => i.Id == SpaceId).FirstOrDefault();
			space.IsApproved=true;
			_spaceSerivce.UpdateSpace(space);
			return View("Index");
		}
		[HttpPost]
		public IActionResult RejectSpace(int spaceId,string message)
		{
			var space = _context.Space.Where(i => i.Id == spaceId).FirstOrDefault();
			space.IsApproved = false;
			_spaceSerivce.UpdateSpace(space);
			return View("Index");
		}
		public async Task <IActionResult> ViewRepest(int requestId)
        {
			var request = _hallRequestService.GetById(requestId);
			var hall =await _hallService.GetByIdAsync(request.HallId,h=>h.HallPhotos);
            var currentHall = _context.SpaceHall.Where(i=>i.Id == hall.Id).FirstOrDefault();
            var space = _context.Space.Where(i => i.Id == currentHall.SpaceId).FirstOrDefault();
			var provider = _context.SpaceEmployee.Where(i => i.SpaceId == space.Id).FirstOrDefault();
            var viewModel = new ReviewSpaceVM
            {
                Space = space,
                spaceEmployee = provider,
                hallRequest = request,
                Hall = hall,
            };
			return View(viewModel);
        }
        [HttpGet]
        public IActionResult AcceptRepest(int requestId)
        {
            var request = _hallRequestService.GetById(requestId);
            var hall = request.Hall;
            hall.Status = HallStatus.Accepted;
            _hallService.UpdateHall(hall);
            _hallRequestService.DeleteRequest(request);
            return View("Index");
        }
        [HttpPost]
        public IActionResult RejectRepest(int requestId, string comment)
        {
            var request = _hallRequestService.GetById(requestId);
            request.comment = comment;
            request.Status = ReqestStatus.Rejected;
            request.Date = DateTime.Now;
            _hallRequestService.UpdateRequest(request);

            return View("Index");
        }


    }
}