using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.ViewModels;
using GetSit.Data.Services;
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
    public class BookingController : Controller
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
        private readonly ICustomerService _customerService;
        public BookingController(AppDBcontext context,
            IUserManager userManager,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ISpaceHallService spaceHallService,
            ISpaceService_Service spaceService_Serivce,
            IBookingHall_Service bookingHall_Service,
            IBookingHallService_Service bookingHallService_Service,
            IPaymentService paymentService,
            IPaymentDetailService paymentDetailService,
            IBookingService bookingService,
            ICustomerService customerService)
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
            _bookingService = bookingService;
            _customerService = customerService;
        }
        [HttpGet,Authorize(Roles ="Customer")]
        public async Task<IActionResult> Index (int HallID)
        {
           

            var hall = _context.SpaceHall.Include(h=> h.HallPhotos )
                .Include(h => h.HallFacilities)
                .Where(o => o.Id == HallID).FirstOrDefault();
            var space = _context.Space.
                Include (s=>s.Services).ThenInclude(h=>h.ServicePhotos)
                .Include(s=>s.WorkingDays)
                .Include(s=>s.Photos)
                .Where(s => s.Halls.Any(h => h.Id == HallID)).FirstOrDefault();


            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var viewModel =  new BookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(HallID, DateTime.Today), 
                
            };


          /* get the available slots for a week from today*/
            viewModel.SlotsForWeek = slots.GetAvailableSlotsForWeek(HallID, DateTime.Today);


            return View(viewModel);
        }
        [HttpPost, Authorize(Roles = "Customer")]
        public async Task<IActionResult> Book(BookingVM viewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.BookingDate = DateTime.Today;

            float NumberOfHours = (float)(viewModel.EndTime - viewModel.StartTime).TotalHours;
            viewModel.TotalCost += viewModel.SelectedHall.CostPerHour * NumberOfHours;

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);
            viewModel.AvailableSlots = slots.GetAvailableSlotsForDay(viewModel.SelectedHall.Id, viewModel.DesiredDate);

            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                var service = _context.SpaceService.FirstOrDefault(s => s.Id == ServiceQuantity.Key);
                viewModel.TotalCost += service.Price * ServiceQuantity.Value; ;
            }

            // Get the current user 
            int id = _userManager.GetCurrentUserId(HttpContext);
            var userobj = _userManager.GetCurrentUserAsync(HttpContext);

            var Booking = new Booking
            {
                CustomerId = id,
                BookingDate = viewModel.BookingDate,
                DesiredDate = viewModel.DesiredDate,
                StartTime = viewModel.StartTime,
                NumberOfHours = NumberOfHours,
                TotalCost = viewModel.TotalCost,
                Paid = 0,
                BookingStatus = BookingStatus.Accepted,
                BookingType = BookingType.Individual,
                
            };
            try
            {
                await _context.Booking.AddAsync(Booking);
                _context.SaveChanges();
               
            }
            catch (Exception error)
            {
                return View(Booking);
            }

            var BookingHall = new BookingHall
            {
                NumberOfUnits = 1,
                PricePerUnit = viewModel.SelectedHall.CostPerHour,
                HallId = viewModel.SelectedHall.Id,
                Hall = viewModel.SelectedHall,
                BookingId = Booking.Id,
                
            };
            try
            {
                await _context.BookingHall.AddAsync(BookingHall);
                _context.SaveChanges();

            }
            catch (Exception error)
            {
                return View(Booking);
            }

            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                var service = await _context.SpaceService.FirstOrDefaultAsync(s => s.Id == ServiceQuantity.Key);
                var BookingHallService = new BookingHallService
                {
                    ServiceId = ServiceQuantity.Key,
                    NumberOfUnits = ServiceQuantity.Value,
                    PricePerUnit = service.Price,
                    BookingHallId = BookingHall.Id,
                    BookingHall = BookingHall,
                    Service = service,

                };

                try
                {
                    await _context.BookingHallService.AddAsync(BookingHallService);
                    _context.SaveChanges();

                }
                catch (Exception error)
                {
                    return View(Booking);
                }
            }
            

            var payment = new Payment
            {
                Status = PaymentStatus.Pending,
                TotalCost = viewModel.TotalCost,
                PaidAmount = 0,
                LastUpdate = DateTime.Now,
                BookingId = Booking.Id,
                Booking = Booking,

            };
            try
            {
                await _context.Payment.AddAsync(payment);
                _context.SaveChanges();

            }
            catch (Exception error)
            {
                return View(Booking);
            }

            var paymentDetail = new PaymentDetail 
            { 
                TotalCost = viewModel.TotalCost,
                Status = PaymentStatus.Pending,
                Type = PaymentType.Cash,
                PaymentId = payment.Id,
                Payment = payment,
                BookingHallId = BookingHall.Id,
                BookingHall = BookingHall,

            };
            try
            {
                await _context.PaymentDetail.AddAsync(paymentDetail);
                _context.SaveChanges();

            }
            catch (Exception error)
            {
                return View(Booking);
            }

            return RedirectToAction("Book");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int bookingId)
        {
            if (bookingId < 1)
                return NotFound();

            var booking = (Booking)_context.Booking.Where(i => i.Id == bookingId)
                .Include(i=>i.Customer)
                .Include(i => i.BookingHalls)
                    .ThenInclude(b => b.BookedServices)
                    .ThenInclude(s=>s.BookingHall)
                .FirstOrDefault();

            if (booking == null)
                return NotFound();

            var userId = _userManager.GetCurrentUserId(HttpContext);
            var userRole = _userManager.GetUserRole(HttpContext);
            #region Security
            UserRole role = UserRole.Customer;
            if (userRole == "Provider")//convert enum userRole to class
            {
                var user = await _providerService.GetByIdAsync(userId);
                role = UserRole.Provider;
                if (booking.BookingHalls.First().Hall.SpaceId != user.SpaceId)
                    return RedirectToAction("AccessDenied", "Account");
            }
            else if(userRole == "Customer")
            {
                var user = await _customerService.GetByIdAsync(userId);
                role = UserRole.Customer;

                if (booking.Customer.Id!= user.Id)
                    return RedirectToAction("AccessDenied", "Account");
            }
            #endregion


            var payment = _paymentSerivce.GetByCustomerBookingId(bookingId);
            

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

            var spaceServices = _serviceService.GetBySpaceId(space.Id);

            var customer = await _customerService.GetByIdAsync(booking.Customer.Id);
            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var endSlots = slots.GetAvailableEndSlots(hall.Id, booking.DesiredDate, booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours)));
    
            var userbooking = new BookingDetailsVM
            {

                HallDetail = halldetail,
                Space = space,
                CustomerBooking=booking,
                customer=customer,
                SpaceServices = spaceServices,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours)),
                Paid = payment.PaidAmount,
                TotalCost = payment.TotalCost,
                servicesDetails = servicesDetails,
                EndSlots = endSlots,
                Role=role
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
            var payment = await _paymentSerivce.GetByIdAsync(PaymentId, p => p.Details, p => p.Booking);
            payment.Details = _paymentDetailService.GetByPaymendId(payment.Id, d => d.BookingHall, d => d.BookingHallService);

            if (payment.Details[0].BookingHall != null)
            {
                payment.Details[0].BookingHall = await _bookingHall_service.GetByIdAsync(payment.Details[0].BookingHall.Id, b => b.Hall);
                if (payment.Details[0].BookingHall.Hall.SpaceId != user.SpaceId)
                    return RedirectToAction("AccessDenied", "Account");
            }
            #endregion
            foreach (var detail in payment.Details)
            {
                if (detail.Status != PaymentStatus.Paid)
                {
                    detail.Status = PaymentStatus.Paid;
                    await _paymentDetailService.UpdateAsync(detail.Id, detail);

                }
            }
            payment.PaidAmount = payment.TotalCost;
            payment.Status = PaymentStatus.Paid;

            await _paymentSerivce.UpdateAsync(payment.Id, payment);
            return RedirectToAction("Details", new { bookingId = payment.BookingId });
        }
        /*To do*/
        [HttpGet,Authorize(Roles="Customer")]
        public async Task<IActionResult> Edit(int bookingId)
        {
            if (bookingId < 1)
                return NotFound();

            var booking = (Booking)_context.Booking.Where(i => i.Id == bookingId)
                .Include(i => i.BookingHalls)
                    .ThenInclude(b => b.BookedServices)
                .FirstOrDefault();
            var payment = _paymentSerivce.GetByCustomerBookingId(bookingId);
            if (booking == null)
                return NotFound();

            var hall = _context.SpaceHall
                .Where(i => i.Id == booking.BookingHalls.First().Id).FirstOrDefault();

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

            var spaceServices = _serviceService.GetBySpaceId(space.Id);

            var customer = await _customerService.GetByIdAsync(booking.CustomerId);
            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var endSlots = slots.GetAvailableEndSlots(hall.Id, booking.DesiredDate, booking.StartTime);

            var userbooking = new BookingDetailsVM
            {
                customer=customer,
                HallDetail = halldetail,
                Space = space,
                SpaceServices = spaceServices,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours)),
                Paid = payment.PaidAmount,
                TotalCost = payment.TotalCost,
                servicesDetails = servicesDetails,
                CustomerBooking = booking,
                EndSlots = endSlots
            };
            return View(userbooking);
        }
        [HttpPost,Authorize(Roles = "Customer")]
        public async Task<IActionResult> Edit(int ID, BookingVM viewModel)
        {
            var Booking = _context.Booking.FirstOrDefault(b => b.Id == ID);

            var hall = _context.SpaceHall
               .Where(i => i.Id == Booking.BookingHalls.First().Id).FirstOrDefault();

            var space = _context.Space
                .Where(s => s.Halls.Any(h => h.Id == hall.Id)).FirstOrDefault();

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var filterDate = viewModel.DesiredDate;
            var IndexModel = new BookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(viewModel.SelectedHall.Id, filterDate),
                FilterDate = filterDate,
                SlotsForWeek = slots.GetAvailableSlotsForWeek(viewModel.SelectedHall.Id, filterDate),
                
            };

            if (!ModelState.IsValid)
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }

            float NumberOfHours = (float)(viewModel.EndTime - viewModel.StartTime).TotalHours;

            if (NumberOfHours <= 0)
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }

            /*Send unavailable error to the employee*/
            if (!slots.IsTimeSlotAvailable(viewModel.SelectedHall.Id, viewModel.DesiredDate, viewModel.StartTime, viewModel.EndTime))
            {
                return RedirectToAction("GetBookingDetails", viewModel);
            }

            // save the new timing in database
            Booking.StartTime = viewModel.StartTime;
            Booking.NumberOfHours = NumberOfHours;
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

                        // update cost 
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

            return RedirectToAction("GetBookingDetails", "Booking");
        }
        [HttpGet, Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelBooking(int ID)
        {
            // Get the current user 
            int id = _userManager.GetCurrentUserId(HttpContext);
            Customer userobj = (Customer)_context.Customer.Where(i => i.Id == id);

            //Get Booking
            Booking booking = (Booking)_context.Booking.Where(i => i.Id == ID);
            TimeSpan difference = booking.DesiredDate - DateTime.UtcNow;
            float days = (float)difference.TotalDays;
            booking.BookingStatus = BookingStatus.Cancelled;
            if (days <= 7 && days >= 3)
            {
                if (booking.BookingDate != DateTime.Today && booking.Paid == 0)
                {
                    userobj.Penality += 50;
                }
                else if (booking.BookingDate != DateTime.Today)
                {
                    userobj.Penality += 30;
                }
                else
                {
                    userobj.Penality += 10;
                }
            }
            else if (days <= 3 && days > 1)
            {
                if (booking.BookingDate != DateTime.Today && booking.Paid == 0)
                {
                    userobj.Penality += 70;
                }
                else if (booking.BookingDate != DateTime.Today)
                {
                    userobj.Penality += 50;
                }
                else
                {
                    userobj.Penality += 30;
                }
            }
            else if (days <= 1)
            {
                if (booking.BookingDate != DateTime.Today && booking.Paid == 0)
                {
                    userobj.Penality += 100;
                }
                else if (booking.BookingDate != DateTime.Today)
                {
                    userobj.Penality += 70;
                }
                else
                {
                    userobj.Penality += 50;
                }
            }
            _context.SaveChanges();
            return View();
        }


    }
}
