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
    [Authorize (policy : "CustomerPolicy")] 
    public class BookingController : Controller
    {
        AppDBcontext _context;
        public BookingController( AppDBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index (int HallID)
        {
           

            var hall = _context.SpaceHall.Include(h=> h.HallPhotos )
                .Include(h => h.HallFacilities)
                .Where(o => o.Id == HallID).FirstOrDefault();
            var space = _context.Space.
                Include (s=>s.Services).ThenInclude(h=>h.ServicePhotos)
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
            List<List<TimeSpan>> availableSlotsForWeek = slots.GetAvailableSlotsForWeek(HallID, DateTime.Today);
            viewModel.SlotsForWeek = availableSlotsForWeek;

                return View(viewModel);
        }

        
        [HttpPost]
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
            UserManager user = new UserManager(_context);
            int id = user.GetCurrentUserId(HttpContext);
            var userobj = user.GetCurrentUser(HttpContext);

            var Booking = new Booking
            {
                CustomerId = id,
                Customer = (Customer)userobj,
                BookingDate = viewModel.BookingDate,
                DesiredDate = viewModel.DesiredDate,
                StartTime = viewModel.StartTime,
                NumberOfHours = NumberOfHours,
                TotalCost = viewModel.TotalCost,
                Paid = 0,
                BookingStatus = BookingStatus.Confirmed,
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
                Booking = Booking,
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
    }
}
