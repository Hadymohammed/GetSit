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
    }
}
