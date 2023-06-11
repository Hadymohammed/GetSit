using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data.ViewModels;

using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

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
                SpaceIdStirng = provider.SpaceId.ToString();
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

        [HttpGet]
       public async Task<IActionResult> GetBookingDetails (object booking)
        {
            if (booking is GuestBooking)
            {
                // get the booking details to be shown
                var obj = (GuestBooking)booking;
                var hall = _context.BookingHall.Where(i => i.BookingId == obj.Id).FirstOrDefault();
                var services = _context.BookingHallService.Where(i => i.BookingHallId == hall.Id).ToList();
                Dictionary<int, int> SelectedServices = new Dictionary<int, int> ();
                List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
                var halldetail = _context.PaymentDetail.Where(i => i.BookingHallId == hall.Id).FirstOrDefault();
                paymentDetails.Add(halldetail);
                foreach (var service in services )
                {
                    SelectedServices.Add(service.Id, service.NumberOfUnits);
                    var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == service.Id).FirstOrDefault();
                    paymentDetails.Add(detail);
                }
                var guestbooking = new GuestBookingVM
                {
                    FirstName = obj.FirstName,
                    LastName = obj.LastName,
                    PhoneNumber = obj.PhoneNumber,
                    BookingDate = obj.BookingDate,
                    DesiredDate = obj.DesiredDate,
                    StartTime = obj.StartTime,
                    EndTime = obj.EndTime,
                    Paid = obj.Paid,
                    TotalCost = obj.TotalCost,
                    SelectedServicesQuantities = SelectedServices,
                    paymentDetails = paymentDetails,
                };
                return View("guestbooking");
            }

            else
            {
                // get the booking details to be shown
                var obj = (Booking)booking;
                var hall = _context.BookingHall.Where(i => i.BookingId == obj.Id).FirstOrDefault();
                var services = _context.BookingHallService.Where(i => i.BookingHallId == hall.Id).ToList();

                Dictionary<int, int> SelectedServices = new Dictionary<int, int>();
                List<PaymentDetail> paymentDetails = new List<PaymentDetail>();
                Dictionary<int, PaymentStatus> ServicesStatus = new Dictionary<int, PaymentStatus>();
                // get the payment details
                var halldetail = _context.PaymentDetail.Where(i => i.BookingHallId == hall.Id).FirstOrDefault();
                paymentDetails.Add(halldetail);
                foreach (var service in services)
                {
                    SelectedServices.Add(service.Id, service.NumberOfUnits);
                    var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == service.Id).FirstOrDefault();
                    ServicesStatus.Add(service.Id, detail.Status);
                    paymentDetails.Add(detail);
                }

                var customer = _context.Customer.Where(i => i.Id == obj.CustomerId).FirstOrDefault();
                TimeSpan endTime = obj.StartTime.Add(TimeSpan.FromHours(obj.NumberOfHours));
                var userbooking = new BookingVM
                {
                    Customer = customer,
                    BookingDate = obj.BookingDate,
                    DesiredDate = obj.DesiredDate,
                    StartTime = obj.StartTime,
                    EndTime = endTime,
                    Paid = obj.Paid,
                    TotalCost = obj.TotalCost,
                    SelectedServicesQuantities = SelectedServices,
                    paymentDetails = paymentDetails,
                };

                return View("userbooking");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(object booking , object BookingviewModel)
        {
            if (booking is GuestBooking)
            {
                var bookingobj = (GuestBooking)booking;
                var viewModel = (GuestBookingVM)BookingviewModel;

                var obj = _context.GuestBooking.Include(b => b.BookingHalls)
                        .ThenInclude(bh => bh.BookedServices)
                            .ThenInclude(bhs => bhs.PaymentDetail)
                                .FirstOrDefault(b => b.Id == bookingobj.Id);

                /* create object from the class to get the available timeslots*/
                AvailableSlots slots = new AvailableSlots(_context);

                if (!ModelState.IsValid)
                {
                    return View(BookingviewModel);
                }

                float NumberOfHours = (float)(viewModel.EndTime - viewModel.StartTime).TotalHours;

                if (NumberOfHours <= 0)
                {
                    return View(BookingviewModel);
                }

                /*Send unavailable error to the employee*/
                if (!slots.IsTimeSlotAvailable(viewModel.SelectedHall.Id, viewModel.DesiredDate, viewModel.StartTime, viewModel.EndTime))
                {
                    return View(BookingviewModel);
                }

                // save the new timing in database
                bookingobj.StartTime = viewModel.StartTime;
                bookingobj.EndTime = viewModel.EndTime;
                _context.SaveChanges();

                bookingobj.TotalCost = viewModel.SelectedHall.CostPerHour * NumberOfHours;

                
                foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
                {
                    SpaceService service = _context.SpaceService.Where(s => s.Id == ServiceQuantity.Key).FirstOrDefault();
                    var booked = _context.BookingHallService.Where(i=>i.ServiceId==service.Id && 
                    i.BookingHallId == viewModel.SelectedHall.Id).FirstOrDefault();
                    if (booked != null)
                    {
                        if (ServiceQuantity.Value > 0)
                        {
                            booked.NumberOfUnits = ServiceQuantity.Value;
                            var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == booked.Id
                            ).FirstOrDefault();

                            bookingobj.TotalCost += service.Price * ServiceQuantity.Value;

                            // update status and cost 

                            detail.Status = viewModel.ServicesStatus[service.Id];
                            detail.TotalCost = service.Price * ServiceQuantity.Value;  

                        }
                        else
                        {
                            //remove this service from booking and its related payment detail
                            var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == booked.Id
                            ).FirstOrDefault();
                            _context.BookingHallService.Remove(booked);
                            _context.PaymentDetail.Remove(detail);
                        }

                    }
                    else if (ServiceQuantity.Value > 0)
                    {
                        viewModel.ServicesStatus.Add(ServiceQuantity.Key, PaymentStatus.Pending);

                        // Add a new service to the booking and related payment detail
                        var BookingHallService = new BookingHallService
                        {
                            ServiceId = ServiceQuantity.Key,
                            NumberOfUnits = ServiceQuantity.Value,
                            PricePerUnit = service.Price,
                            BookingHallId = viewModel.SelectedHall.Id,
                           // BookingHall = (object) viewModel.SelectedHall,
                            Service = service,
                        };

                        var paymentServiceDetail = new PaymentDetail
                        {
                            TotalCost = service.Price * ServiceQuantity.Value,
                            Status = PaymentStatus.Pending,
                            Type = PaymentType.Cash,
                            BookingHallServiceId = BookingHallService.Id
                        };

                        bookingobj.TotalCost += service.Price * ServiceQuantity.Value;

                       
                    }
                        _context.SaveChanges();
                }

            }

            // booking by customer
            else
            {
                var bookingobj = (Booking)booking;
                var viewModel = (BookingVM)BookingviewModel;
                foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
                {
                    SpaceService service = _context.SpaceService.Where(s => s.Id == ServiceQuantity.Key).FirstOrDefault();
                    var booked = _context.BookingHallService.Where(i => i.ServiceId == service.Id &&
                    i.BookingHallId == viewModel.SelectedHall.Id).FirstOrDefault();
                    if (booked != null)
                    {
                        var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == booked.Id
                            ).FirstOrDefault();

                        // update status and cost 

                        detail.Status = viewModel.ServicesStatus[service.Id];
                       
                    }
                    
                }
                _context.SaveChanges();

            }

            return RedirectToAction("GetBookingDetails", "SpaceManagement");
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
