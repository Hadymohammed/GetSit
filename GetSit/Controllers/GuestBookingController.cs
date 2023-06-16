using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data.ViewModels;
using GetSit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Security.Claims;
using static System.Reflection.Metadata.BlobBuilder;

namespace GetSit.Controllers
{
    public class GuestBookingController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        private readonly ISpaceEmployeeService _providerService;
        private readonly ISpaceService _spaceService;
        private readonly ISpaceHallService _hallService;
        private readonly ISpaceService_Service _serviceService;
        private readonly IBookingHall_Service _bookingHall_service;
        private readonly IBookingHallService_Service _bookingService_Serivce;
        private readonly IPaymentService _paymentSerivce;
        private readonly IPaymentDetailService _paymentDetailService;
        public GuestBookingController(AppDBcontext context,
            IUserManager userManager,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService spaceHallService,
            ISpaceService_Service spaceService_Serivce,
            IBookingHall_Service bookingHall_Service,
            IBookingHallService_Service bookingHallService_Service,
            IPaymentService paymentService,
            IPaymentDetailService paymentDetailService)
        {
            _context = context;
            _userManager = userManager;
            _providerService = spaceEmployeeService;
            _spaceService = spaceService;
            _serviceService = spaceService_Serivce;
            _hallService = spaceHallService;
            _bookingHall_service = bookingHall_Service;
            _bookingService_Serivce = bookingHallService_Service;
            _paymentSerivce = paymentService;
            _paymentDetailService = paymentDetailService;
        }
        public static void GetHoursAndMinutes(string timeSpanString, out int hours, out int minutes)
        {
            DateTime time;
            if (DateTime.TryParseExact(timeSpanString, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out time))
            {
                hours = time.Hour;
                minutes = time.Minute;
            }
            else
            {
                hours = 0;
                minutes = 0;
            }
        }
        [HttpGet]
        public async Task<IActionResult> Index(int HallId, DateTime? date)
        {

            // Get the current user 
            int id = _userManager.GetCurrentUserId(HttpContext);
            var emp = await _providerService.GetByIdAsync(id);
           
            var hall = await _hallService.GetByIdAsync(HallId,h=>h.HallFacilities,h=>h.HallPhotos);
            if (HallId < 1 || hall == null)
                return NotFound();
            if (hall.SpaceId != emp.SpaceId)
                return RedirectToAction("AccessDenied", "Account");

            var space = await _spaceService.GetByIdAsync(hall.SpaceId, s => s.Services,
                s => s.Services,
                s => s.WorkingDays,
                s => s.Photos);
            space.Services = _serviceService.GetBySpaceId(hall.SpaceId, s => s.ServicePhotos);
            AvailableSlots slots = new AvailableSlots(_context);
            var filterDate = (date != null) ? (DateTime)date : DateTime.Today;
            var viewModel = new GuestBookingVM
            {
                SelectedSpace = space,
                HallId = hall.Id,
                SelectedHall = hall,
                FilterDate = filterDate,
                AvailableSlots = slots.GetAvailableSlotsForDay(hall.Id, filterDate),
                SlotsForWeek= slots.GetAvailableSlotsForWeek(hall.Id, filterDate)
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(GuestBookingVM viewModel)
        {
            int id = _userManager.GetCurrentUserId(HttpContext);
            var emp = await _providerService.GetByIdAsync(id);
            #region GET view model for errors

            var hall = await _hallService.GetByIdAsync(viewModel.HallId, h => h.HallFacilities, h => h.HallPhotos);
            if (viewModel.HallId < 1 || hall == null)
                return NotFound();
            if (hall.SpaceId != emp.SpaceId)
                return RedirectToAction("AccessDenied", "Account");

            var space = await _spaceService.GetByIdAsync(hall.SpaceId, s => s.Services,
                s => s.Services,
                s => s.WorkingDays,
                s => s.Photos);
            space.Services = _serviceService.GetBySpaceId(hall.SpaceId, s => s.ServicePhotos);
            AvailableSlots slots = new AvailableSlots(_context);
            var filterDate = viewModel.DesiredDate;
            var IndexModel = new GuestBookingVM
            {
                SelectedSpace = space,
                HallId = hall.Id,
                SelectedHall = hall,
                AvailableSlots = slots.GetAvailableSlotsForDay(hall.Id, filterDate),
                SlotsForWeek = slots.GetAvailableSlotsForWeek(hall.Id, filterDate),
                FilterDate = filterDate
            };



            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DesiredDate", "Make sure you press the Check availability then choose your timing.");
                return View(IndexModel);
            }
            #endregion
            int startH = 0, startM = 0; GetHoursAndMinutes(viewModel.StartTime, out startH, out startM);
            TimeSpan start = new TimeSpan(startH, startM, 0);
            int endH = 0, endM = 0; GetHoursAndMinutes(viewModel.EndTime, out endH, out endM);
            TimeSpan end = new TimeSpan(endH, endM, 0);

            float NumberOfHours = (float)(end - start).TotalHours;

            if (NumberOfHours <= 0)
            {
                return View(IndexModel);
            }

            /*Send unavailable error to the employee*/
            if (!slots.IsTimeSlotAvailable(hall.Id, viewModel.DesiredDate, start, end))
            {
                return View(IndexModel);
            }

            // save the booking details into the database
            try
            {
                // Add Guest Booking
                var GuestBooking = new GuestBooking
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    PhoneNumber = viewModel.PhoneNumber,
                    EmployeeId = id,
                    BookingDate = DateTime.Now,
                    DesiredDate = viewModel.DesiredDate,
                    StartTime = start,
                    EndTime = end,
                    TotalCost = 0,
                    Paid = 0,
                    BookingStatus = BookingStatus.Confirmed,
                };
                await _context.GuestBooking.AddAsync(GuestBooking);
                await _context.SaveChangesAsync();

                // Add Payment
                var payment = new Payment
                {
                    Status = PaymentStatus.Pending,
                    TotalCost = 0,
                    PaidAmount = 0,
                    LastUpdate = DateTime.Now,
                    GuestBookingId = GuestBooking.Id,
                    isGuest=true
                };
                await _paymentSerivce.AddAsync(payment);

                // Add Booked hall with its paymrnt detail
                var BookingHall = new BookingHall
                {
                    NumberOfUnits = 1,
                    PricePerUnit = hall.CostPerHour,
                    HallId = hall.Id,
                    GuestBookingId = GuestBooking.Id,
                    isGuest = true
                };
                await _bookingHall_service.AddAsync(BookingHall);

                var paymentHallDetail = new PaymentDetail
                {
                    TotalCost = hall.CostPerHour * NumberOfHours,
                    Status = PaymentStatus.Pending,
                    Type = PaymentType.Cash,
                    PaymentId = payment.Id,
                    BookingHallId = BookingHall.Id,
                };
                await _paymentDetailService.AddAsync(paymentHallDetail);

                GuestBooking.TotalCost += hall.CostPerHour * NumberOfHours;

                // Add Selected Services each with its payment detail
                foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
                {
                    if (ServiceQuantity.Value == 0) 
                    {
                        continue;
                    }
                        
                    var service = await _serviceService.GetByIdAsync(ServiceQuantity.Key);
                    var BookingHallService = new BookingHallService
                    {
                        ServiceId = ServiceQuantity.Key,
                        NumberOfUnits = ServiceQuantity.Value,
                        PricePerUnit = service.Price,
                        BookingHallId = BookingHall.Id,
                    };
                    await _bookingService_Serivce.AddAsync(BookingHallService);

                    var paymentServiceDetail = new PaymentDetail
                    {
                        TotalCost = service.Price * ServiceQuantity.Value,
                        Status = PaymentStatus.Pending,
                        Type = PaymentType.Cash,
                        BookingHallServiceId = BookingHallService.Id,
                        PaymentId=payment.Id
                    };
                    await _paymentDetailService.AddAsync(paymentServiceDetail);

                    GuestBooking.TotalCost += service.Price * ServiceQuantity.Value;
                }

                payment.TotalCost = GuestBooking.TotalCost;
                await _paymentSerivce.UpdateAsync(payment.Id, payment);
                return RedirectToAction("Index", "SpaceManagement");
            }
            catch (Exception err)
            {
                ModelState.AddModelError("DesiredDate", "Unexpected error,Try again.");
                return View(IndexModel);
            }

            return View(IndexModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int bookingId)
        {
            if (bookingId < 1)
                return NotFound();

            var booking = (GuestBooking)_context.GuestBooking.Where(i => i.Id == bookingId)
                .Include(i => i.BookingHalls)
                    .ThenInclude(b => b.BookedServices)
                .FirstOrDefault();
            var payment = _paymentSerivce.GetByBookingId(bookingId);
            if (booking == null)
                return NotFound();

            var hall = _context.SpaceHall
                .Where(i => i.Id == booking.BookingHalls.First().Id).FirstOrDefault();

            var space = await _spaceService.GetByIdAsync(hall.SpaceId);

            var halldetail = _context.PaymentDetail
                .Include(d=>d.BookingHall)
                    .ThenInclude(b=>b.Hall)
                        .ThenInclude(h=>h.HallPhotos)
                .FirstOrDefault(i => i.BookingHallId == booking.BookingHalls.First().Id);

            var servicesDetails = _context.PaymentDetail
                                .Include(d => d.BookingHallService)
                                    .ThenInclude(s => s.Service)
                                        .ThenInclude(ss=>ss.ServicePhotos)
                                .Where(d => d.PaymentId == payment.Id).ToList();
            servicesDetails.RemoveAt(0);

            var employee = await _providerService.GetByIdAsync(booking.EmployeeId);

            var spaceServices = _serviceService.GetBySpaceId(space.Id);

         
            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);


            var userbooking = new BookingDetailsVM
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                PhoneNumber = booking.PhoneNumber,
                HallDetail = halldetail,
                Space = space,
                SpaceServices=spaceServices,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Paid = booking.Paid,
                TotalCost = booking.TotalCost,
                servicesDetails = servicesDetails,
                Employee=employee,
                Booking=booking
            };
            return View(userbooking);
        }

        [HttpPost]
        public async Task<IActionResult> EditBooking(int ID, GuestBookingVM viewModel)
        {
            var Booking = _context.GuestBooking.FirstOrDefault(b => b.Id == ID);

            var hall = _context.SpaceHall
               .Where(i => i.Id == Booking.BookingHalls.First().Id).FirstOrDefault();

            var space = _context.Space
                .Where(s => s.Halls.Any(h => h.Id == hall.Id)).FirstOrDefault();

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var filterDate = viewModel.DesiredDate;
            var IndexModel = new GuestBookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(viewModel.SelectedHall.Id, filterDate),
                FilterDate = filterDate,
                SlotsForWeek = slots.GetAvailableSlotsForWeek(viewModel.SelectedHall.Id, filterDate)
            };



            if (!ModelState.IsValid)
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }
            int startH = 0, startM = 0; GetHoursAndMinutes(viewModel.StartTime, out startH, out startM);
            TimeSpan start = new TimeSpan(startH, startM, 0);
            int endH = 0, endM = 0; GetHoursAndMinutes(viewModel.EndTime, out endH, out endM);
            TimeSpan end = new TimeSpan(endH, endM, 0);

            float NumberOfHours = (float)(end - start).TotalHours;
            
            if (NumberOfHours <= 0)
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }

            /*Send unavailable error to the employee*/
            if (!slots.IsTimeSlotAvailable(viewModel.SelectedHall.Id, viewModel.DesiredDate, start, end))
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }

            // save the new timing in database
            Booking.StartTime =start;
            Booking.EndTime = end;
            _context.SaveChanges();

            Booking.TotalCost = viewModel.SelectedHall.CostPerHour * NumberOfHours;

            PaymentDetail halldetail = (PaymentDetail)_context.PaymentDetail.Where(i => i.BookingHallId == hall.Id).FirstOrDefault();

            if (halldetail.Status == PaymentStatus.Paid)
            {
                Booking.Paid = hall.CostPerHour * NumberOfHours;
            }
            else
            {
                Booking.Paid = 0;
            }

            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                SpaceService service = _context.SpaceService.Where(s => s.Id == ServiceQuantity.Key).FirstOrDefault();
                var booked = _context.BookingHallService.Where(i => i.ServiceId == service.Id &&
                i.BookingHallId == viewModel.SelectedHall.Id).FirstOrDefault();
                if (booked != null)
                {
                    if (ServiceQuantity.Value > 0)
                    {
                        booked.NumberOfUnits = ServiceQuantity.Value;
                        var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == booked.Id
                        ).FirstOrDefault();

                        Booking.TotalCost += service.Price * ServiceQuantity.Value;

                        // update status and cost 
                        detail.Status = viewModel.ServicesStatus[service.Id];
                        detail.TotalCost = service.Price * ServiceQuantity.Value;
                        if (viewModel.ServicesStatus[service.Id] == PaymentStatus.Paid)
                        {
                            Booking.Paid += detail.TotalCost;
                        }

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
                        Service = service,
                    };

                    var paymentServiceDetail = new PaymentDetail
                    {
                        TotalCost = service.Price * ServiceQuantity.Value,
                        Status = PaymentStatus.Pending,
                        Type = PaymentType.Cash,
                        BookingHallServiceId = BookingHallService.Id
                    };

                    Booking.TotalCost += service.Price * ServiceQuantity.Value;


                }
                _context.SaveChanges();
            }

            return RedirectToAction("GetBookingDetails", "CustomerAccount");
        }

        public async Task<IActionResult> CancelBooking(int ID)
        {
            
            var booking = _context.GuestBooking.Where (i=>i.Id == ID).FirstOrDefault();
            booking.BookingStatus = BookingStatus.Cancelled;
            _context.SaveChanges();

            
            return View("Index", "SpaceManagement");
        }
    }

}
