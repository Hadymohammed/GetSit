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
using System.Globalization;

namespace GetSit.Controllers
{
    [Authorize(Roles = "Provider")]
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
        private readonly IBookingService _bookingService;
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
            var hall = await _hallService.GetByIdAsync(HallId, h => h.HallFacilities, h => h.HallPhotos);
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
                SlotsForWeek = slots.GetAvailableSlotsForWeek(hall.Id, filterDate)
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
            if (viewModel.DesiredDate.Date< DateTime.Now.Date)
            {
                ModelState.AddModelError("DesiredDate", "Choose a valid booking date.");
                return View(IndexModel);
            }
            #endregion

            int startH = 0, startM = 0; GetHoursAndMinutes(viewModel.StartTime, out startH, out startM);
            TimeSpan start = new TimeSpan(startH, startM, 0);
            int endH = 0, endM = 0; GetHoursAndMinutes(viewModel.EndTime, out endH, out endM);
            TimeSpan end = new TimeSpan(endH, endM, 0);

            if(start.Ticks < DateTime.Now.TimeOfDay.Ticks)
            {
                ModelState.AddModelError("StartTime", "Choose a valid booking time.");
                return View(IndexModel);

            }

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
                    BookingStatus = BookingStatus.Accepted,
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
                    isGuest = true
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
                        PaymentId = payment.Id
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

            var hall = await _hallService.GetByIdAsync(booking.BookingHalls.First().HallId);
                
            var space = await _spaceService.GetByIdAsync(hall.SpaceId);

            var halldetail = _context.PaymentDetail
                .Include(d => d.BookingHall)
                    .ThenInclude(b => b.Hall)
                        .ThenInclude(h => h.HallPhotos)
                .FirstOrDefault(i => i.BookingHallId == booking.BookingHalls.First().Id);

            var servicesDetails = _context.PaymentDetail
                                .Include(d => d.BookingHallService)
                                    .ThenInclude(s => s.Service)
                                        .ThenInclude(ss => ss.ServicePhotos)
                                .Where(d => d.PaymentId == payment.Id).ToList();
            servicesDetails.RemoveAt(0);

            var employee = await _providerService.GetByIdAsync(booking.EmployeeId);

            var spaceServices = _serviceService.GetBySpaceId(space.Id);


            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);
            var endSlots = slots.GetAvailableEndSlots(hall.Id, booking.DesiredDate, booking.StartTime);

            var userbooking = new BookingDetailsVM
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                PhoneNumber = booking.PhoneNumber,
                HallDetail = halldetail,
                Space = space,
                SpaceServices = spaceServices,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Paid = payment.PaidAmount,
                TotalCost = payment.TotalCost,
                servicesDetails = servicesDetails,
                Employee = employee,
                Booking = booking,
                EndSlots = endSlots
            };
            return View(userbooking);
        }

        [HttpGet, Authorize(Roles = "Provider")]
        public async Task<IActionResult> Pay(int PaymentId)
        {
            if (PaymentId < 1)
                return NotFound();
            #region security
            var userId = _userManager.GetCurrentUserId(HttpContext);
            var user = await _providerService.GetByIdAsync(userId);
            var payment = await _paymentSerivce.GetByIdAsync(PaymentId, p => p.Details,p=>p.GuestBooking);
            payment.Details =  _paymentDetailService.GetByPaymendId(payment.Id, d => d.BookingHall, d => d.BookingHallService);
            
            if (payment.Details[0].BookingHall != null)
            {
                payment.Details[0].BookingHall = await _bookingHall_service.GetByIdAsync(payment.Details[0].BookingHall.Id, b => b.Hall);
                if (payment.Details[0].BookingHall.Hall.SpaceId != user.SpaceId)
                    return RedirectToAction("AccessDenied", "Account");
            }
            #endregion
            foreach(var detail in payment.Details)
            {
                if (detail.Status != PaymentStatus.Paid)
                {
                    detail.Status = PaymentStatus.Paid;
                    var x= detail.TotalCost;
                    await _paymentDetailService.UpdateAsync(detail.Id, detail);

                }
            }
            payment.PaidAmount = payment.TotalCost;
            payment.Status = PaymentStatus.Paid;

            await _paymentSerivce.UpdateAsync(payment.Id, payment);
            return RedirectToAction("Details", new { bookingId = payment.GuestBookingId });
        }
        //To Do//
        [HttpGet]
        public async Task<IActionResult> Edit(int bookingId)
        {
            if (bookingId < 1)
                return NotFound();

            var booking = (GuestBooking)_context.GuestBooking.Where(i => i.Id == bookingId)
                .Include(i => i.BookingHalls)
                    .ThenInclude(b=>b.Hall)
                .FirstOrDefault();

            var payment = _paymentSerivce.GetByBookingId(bookingId);
            if (booking == null)
                return NotFound();

            var hall = _context.SpaceHall
                .Where(i => i.Id == booking.BookingHalls.First().Hall.Id).FirstOrDefault();

            var space = await _spaceService.GetByIdAsync(hall.SpaceId);

            var halldetail = _context.PaymentDetail
                .Include(d => d.BookingHall)
                    .ThenInclude(b => b.Hall)
                        .ThenInclude(h => h.HallPhotos)
                .FirstOrDefault(i => i.BookingHallId == booking.BookingHalls.First().Id);

            var servicesDetails = _context.PaymentDetail
                                .Include(d => d.BookingHallService)
                                    .ThenInclude(s => s.Service)
                                        .ThenInclude(ss => ss.ServicePhotos)
                                .Where(d => d.PaymentId == payment.Id).ToList();
            servicesDetails.RemoveAt(0);

            var employee = await _providerService.GetByIdAsync(booking.EmployeeId);

            var spaceServices = _serviceService.GetBySpaceId(space.Id);


            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var endSlots = slots.GetAvailableEndSlots(hall.Id, booking.DesiredDate, booking.EndTime);

            var userbooking = new BookingDetailsVM
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                PhoneNumber = booking.PhoneNumber,
                HallDetail = halldetail,
                Space = space,
                SpaceServices = spaceServices,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Paid = payment.PaidAmount,
                TotalCost = payment.TotalCost,
                servicesDetails = servicesDetails,
                Employee = employee,
                Booking = booking,
                EndSlots = endSlots,
                HallId=hall.Id
            };
            return View(userbooking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBookingPostVM viewModel)
        {
            var Booking = _context.GuestBooking.Where(b => b.Id == viewModel.BookingId)
                .Include(b=>b.Payment)
                .Include(b=>b.BookingHalls)
                    .ThenInclude(bb=>bb.BookedServices)
                .FirstOrDefault();

            var hall = await _hallService.GetByIdAsync((int)viewModel.HallId);

            var space = await _spaceService.GetByIdAsync(hall.SpaceId);

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);
            
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", new { bookingId = viewModel.BookingId});
            }
            Booking.FirstName = viewModel.FirstName;
            Booking.LastName = viewModel.LastName;
            Booking.PhoneNumber = viewModel.PhoneNumber;

            int endH = 0, endM = 0; GetHoursAndMinutes(viewModel.EndTime, out endH, out endM);
            TimeSpan end = new TimeSpan(endH, endM, 0);
            TimeSpan start = Booking.EndTime;

            float NumberOfHours = (float)(end - start).TotalHours;
            var payment = await _paymentSerivce.GetByIdAsync(Booking.Payment.Id);
            
            if (NumberOfHours < 0)
            {
                ModelState.AddModelError("EndTime", "Select valid end time.");
                return RedirectToAction("Details", new {BookingId=Booking.Id});
            }
            if (NumberOfHours != 0)
            {
                // Send unavailable error to the employee
                if (!slots.IsTimeSlotAvailable((int)viewModel.HallId, Booking.DesiredDate, start, end))
                {
                    return RedirectToAction("Details", new { BookingId = viewModel.BookingId });
                }
                // save the new timing in database
                Booking.EndTime = end;
                Booking.TotalCost += hall.CostPerHour * NumberOfHours;
                PaymentDetail halldetail = (PaymentDetail)_context.PaymentDetail.Where(i => i.BookingHallId == Booking.BookingHalls.First().Id).FirstOrDefault();
           
                halldetail.TotalCost += hall.CostPerHour * NumberOfHours;

                if (NumberOfHours!=0&&halldetail.Status == PaymentStatus.Paid)
                {
                    halldetail.Status= PaymentStatus.Uncompleted;
                }
                await _paymentDetailService.UpdateAsync(halldetail.Id, halldetail);
            }
            
            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                SpaceService service = (SpaceService)await _serviceService.GetByIdAsync(ServiceQuantity.Key);
                #region updateBookedServices
                /*
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
                */
                #endregion
                
                if (ServiceQuantity.Value > 0)
                {
                    // Add a new service to the booking and related payment detail
                    var BookingHallService = new BookingHallService
                    {
                        ServiceId = ServiceQuantity.Key,
                        NumberOfUnits = ServiceQuantity.Value,
                        PricePerUnit = service.Price,
                        BookingHallId = Booking.BookingHalls.First().Id,
                        Service = service,
                    };
                    await _bookingService_Serivce.AddAsync(BookingHallService);

                    var paymentServiceDetail = new PaymentDetail
                    {
                        PaymentId=payment.Id,
                        TotalCost = service.Price * ServiceQuantity.Value,
                        Status = PaymentStatus.Pending,
                        Type = PaymentType.Cash,
                        BookingHallServiceId = BookingHallService.Id
                    };
                    await _paymentDetailService.AddAsync(paymentServiceDetail);

                    Booking.TotalCost += service.Price * ServiceQuantity.Value;
                }
            }
            payment.TotalCost = Booking.TotalCost;
            _context.GuestBooking.Update(Booking);
            await _paymentSerivce.UpdateAsync(payment.Id, payment);

            return RedirectToAction("Details", new {BookingId=Booking.Id});
        }
        [HttpGet]
        public async Task<IActionResult> Cancel(int BookingId)
        {

            var booking = _context.GuestBooking.Where(i => i.Id == BookingId).FirstOrDefault();
            
            booking.BookingStatus = BookingStatus.Cancelled;
            _context.GuestBooking.Update(booking);
            _context.SaveChanges();

            return RedirectToAction("Index","SpaceManagement");
        }
    }

}
