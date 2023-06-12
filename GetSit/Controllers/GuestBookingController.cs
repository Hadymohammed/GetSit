using GetSit.Common;
using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Data.Security;
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
        public GuestBookingController(AppDBcontext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index( DateTime? date)
        {
            // Get the current user 
            int id = _userManager.GetCurrentUserId(HttpContext);
            var emp = _context.SpaceEmployee.Where(x => x.Id == id).FirstOrDefault();
           
            var space = _context.Space.
                Include(s => s.Services).ThenInclude(h => h.ServicePhotos)
                .Include(s => s.WorkingDays)
                .Include(s => s.Photos)
                .Where(s => s.Id == emp.SpaceId).FirstOrDefault();

            var viewModel = new GuestBookingVM
            {
                SelectedSpace = space,
                FilterDate = (date != null) ? (DateTime)date : DateTime.Today,

            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GuestBook(GuestBookingVM viewModel)
        {
            int id = _userManager.GetCurrentUserId(HttpContext);
            var emp = _context.SpaceEmployee.Where(x => x.Id == id).FirstOrDefault();

            var space = _context.Space.
                Include(s => s.Services).ThenInclude(h => h.ServicePhotos)
                .Include(s => s.WorkingDays)
                .Include(s => s.Photos)
                .Where(s => s.Id == emp.SpaceId).FirstOrDefault();

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var IndexModel = new GuestBookingVM
            {
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(viewModel.SelectedHall.Id, viewModel.DesiredDate),
                SlotsForWeek = slots.GetAvailableSlotsForWeek(viewModel.SelectedHall.Id, viewModel.DesiredDate),
                FilterDate = viewModel.DesiredDate
            };



            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DesiredDate", "Make sure you press the Check availability then choose your timing.");
                return View(IndexModel);
            }


            float NumberOfHours = (float)(viewModel.EndTime - viewModel.StartTime).TotalHours;

            if (NumberOfHours <= 0)
            {
                return View(IndexModel);
            }

            /*Send unavailable error to the employee*/
            if (!slots.IsTimeSlotAvailable(viewModel.SelectedHall.Id, viewModel.DesiredDate, viewModel.StartTime, viewModel.EndTime))
            {
                return View(IndexModel);
            }

            viewModel.BookingDate = DateTime.Today;
 

            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                var service = _context.SpaceService.FirstOrDefault(s => s.Id == ServiceQuantity.Key);
                viewModel.TotalCost += service.Price * ServiceQuantity.Value; ;
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
                    Employee = emp,
                    BookingDate = viewModel.BookingDate,
                    DesiredDate = viewModel.DesiredDate,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    TotalCost = 0,
                    Paid = 0,
                    BookingStatus = BookingStatus.Confirmed,
                };
                await _context.GuestBooking.AddAsync(GuestBooking);
                _context.SaveChanges();

                // Add Payment
                var payment = new Payment
                {
                    Status = PaymentStatus.Pending,
                    TotalCost = 0,
                    PaidAmount = 0,
                    LastUpdate = DateTime.Now,
                    GuestBookingId = GuestBooking.Id,
                    GuestBooking = GuestBooking,
                };
                await _context.Payment.AddAsync(payment);
                _context.SaveChanges();

                // Add Booked hall with its paymrnt detail
                var BookingHall = new BookingHall
                {
                    NumberOfUnits = 1,
                    PricePerUnit = viewModel.SelectedHall.CostPerHour,
                    HallId = viewModel.SelectedHall.Id,
                    Hall = viewModel.SelectedHall,
                    GuestBookingId = GuestBooking.Id,
                };
                await _context.BookingHall.AddAsync(BookingHall);
                _context.SaveChanges();

                var paymentHallDetail = new PaymentDetail
                {
                    TotalCost = viewModel.SelectedHall.CostPerHour * NumberOfHours,
                    Status = PaymentStatus.Pending,
                    Type = PaymentType.Cash,
                    PaymentId = payment.Id,
                    BookingHallId = BookingHall.Id,
                };
                await _context.PaymentDetail.AddAsync(paymentHallDetail);
                _context.SaveChanges();

                GuestBooking.TotalCost += viewModel.SelectedHall.CostPerHour * NumberOfHours;

                // Add Selected Services each with its payment detail
                foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
                {
                    if (ServiceQuantity.Value == 0) 
                    {
                        continue;
                    }
                        
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

                    var paymentServiceDetail = new PaymentDetail
                    {
                        TotalCost = service.Price * ServiceQuantity.Value,
                        Status = PaymentStatus.Pending,
                        Type = PaymentType.Cash,
                        BookingHallServiceId = BookingHallService.Id
                    };

                    await _context.BookingHallService.AddAsync(BookingHallService);
                    await _context.PaymentDetail.AddAsync(paymentServiceDetail);
                    _context.SaveChanges();

                    GuestBooking.TotalCost += service.Price * ServiceQuantity.Value;
                }

                payment.TotalCost = GuestBooking.TotalCost;

            }
            catch (Exception err)
            {
                ModelState.AddModelError("DesiredDate", "Unexpected error,Try again.");
                return View(IndexModel);
            }

            return RedirectToAction("GuestBook");
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingDetails(int ID)
        {
            GuestBooking booking = (GuestBooking)_context.GuestBooking.Include(i => i.BookingHalls).ThenInclude(i => i.BookedServices)
                .Where(i => i.Id == ID).FirstOrDefault();

            var hall = _context.SpaceHall
                .Where(i => i.Id == booking.BookingHalls.First().Id).FirstOrDefault();

            var space = _context.Space
                .Where(s => s.Halls.Any(h => h.Id == hall.Id)).FirstOrDefault();

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


            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var filterDate = booking.DesiredDate;

            var userbooking = new GuestBookingVM
            {
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                PhoneNumber = booking.PhoneNumber,
                SelectedHall = hall,
                SelectedSpace = space,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                Paid = booking.Paid,
                TotalCost = booking.TotalCost,
                SelectedServicesQuantities = SelectedServices,
                paymentDetails = paymentDetails,
                FilterDate = filterDate,
                AvailableSlots = slots.GetAvailableSlotsForDay(hall.Id, filterDate),
                SlotsForWeek = slots.GetAvailableSlotsForWeek(hall.Id, filterDate)
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
            Booking.EndTime = viewModel.EndTime;
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
