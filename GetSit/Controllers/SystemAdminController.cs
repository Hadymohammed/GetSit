using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data;

using Microsoft.AspNetCore.Mvc;
using GetSit.Data.enums;

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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewRepest(int requestId)
        {
            return View();
        }
        [HttpPost]
        public IActionResult AcceptRepest(int requestId)       {
            var request= _hallRequestService.GetById(requestId);
            var hall =request.Hall;
            hall.Status = HallStatus.Accepted;
            _hallService.UpdateHall(hall);
            _hallRequestService.DeleteRequest(request);
            return View("Index");
        }

        [HttpPost]
        public IActionResult RejectRepest(int requestId,string comment)
        {
            var request = _hallRequestService.GetById(requestId);
            request.comment = comment;
            request.Status = ReqestStatus.Rejected;
            request.Date = DateTime.Now();
            _hallRequestService.UpdateRequest(request);
            
            return View("Index");
        }
    }
}
