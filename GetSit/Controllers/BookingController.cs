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
    [Authorize (policy : "CustomerPolicy")] 
    public class BookingController : Controller
    {
        #region Inject Dependncies
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        private readonly IBookingService _bookingService;
        private readonly ISpaceService _spaceService;
        private readonly ISpaceHallService _hallSerivce;
        private readonly IBookingHall_Service _bookingHall_service;
        private readonly ISpaceService_Service _spaceService_Service;
        private readonly IBookingHallService_Service _bookingService_Serivce;
        private readonly IPaymentService _paymentSerivce;
        private readonly IPaymentDetailService _paymentDetailService;
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
        public BookingController( AppDBcontext context,
            IUserManager userManager,
            IBookingService bookingService,
            ISpaceHallService spaceHallService,
            IBookingHall_Service bookingHall_service,
            ISpaceService_Service spaceService_Service,
            IBookingHallService_Service bookingService_Serivce,
            IPaymentService paymentService,
            IPaymentDetailService paymentDetailService,
            ISpaceService spaceService)
        {
            _context = context;
            _userManager = userManager;
            _bookingService = bookingService;
            _hallSerivce = spaceHallService;
            _bookingHall_service = bookingHall_service;
            _spaceService_Service = spaceService_Service;
            _bookingService_Serivce = bookingService_Serivce;
            _paymentSerivce= paymentService;
            _paymentDetailService= paymentDetailService;
            _spaceService= spaceService;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Index(int HallID,DateTime? date)
        {
            if(HallID==0)
                return RedirectToAction("Index","Explore");

            var hall = await _hallSerivce.GetByIdAsync(HallID, h => h.HallPhotos, hall => hall.HallFacilities);
            var space = await _spaceService.GetByIdAsync(hall.SpaceId, s => s.Services,
                s => s.Services,
                s => s.WorkingDays,
                s => s.Photos);
            space.Services = _spaceService_Service.GetBySpaceId(hall.SpaceId,s=>s.ServicePhotos);

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);
            var filterDate = DateTime.Today;
            if (date != null)
                filterDate = (DateTime)date;
            var viewModel =  new BookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(HallID, filterDate), 
                FilterDate=filterDate
                
            };


          /* get the available slots for a week from today*/
            viewModel.SlotsForWeek = slots.GetAvailableSlotsForWeek(HallID, filterDate);


            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Index(CreateCustomerBookingVM viewModel)
        {
            #region GET view model for errors
            var hall = await _hallSerivce.GetByIdAsync(viewModel.HallId, h => h.HallPhotos, hall => hall.HallFacilities);
            var space = await _spaceService.GetByIdAsync(hall.SpaceId, s => s.Services,
                s => s.Services,
                s => s.WorkingDays,
                s => s.Photos);
            space.Services = _spaceService_Service.GetBySpaceId(hall.SpaceId, s => s.ServicePhotos);

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);
            var filterDate = viewModel.DesiredDate;
            var IndexModel = new BookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                AvailableSlots = slots.GetAvailableSlotsForDay(viewModel.HallId, filterDate),
                FilterDate = filterDate

            };
            IndexModel.SlotsForWeek = slots.GetAvailableSlotsForWeek(viewModel.HallId, filterDate);

            #endregion

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DesiredDate", "Make sure you press the Check availability then choose your timing.");
                return View(IndexModel);
            }
            int startH = 0, startM = 0; GetHoursAndMinutes(viewModel.StartTime, out startH, out startM);
            TimeSpan start = new TimeSpan(startH, startM, 0);
            int endH = 0, endM = 0; GetHoursAndMinutes(viewModel.EndTime, out endH, out endM);
            TimeSpan end = new TimeSpan(endH, endM, 0);

            float NumberOfHours = (float)(end - start).TotalHours;
            if(NumberOfHours<=0)
                return View(IndexModel);

            /*Send unavailable error to client*/
            if (!slots.IsTimeSlotAvailable(viewModel.HallId, viewModel.DesiredDate, start, end))
            {
                return View(IndexModel);
            }
            // Get the current user 
            int customerId = _userManager.GetCurrentUserId(HttpContext);
            try
            {
                /*CreateBooking*/
                var Booking = new Booking
                {
                    CustomerId = customerId,
                    BookingDate = DateTime.Now,
                    DesiredDate = viewModel.DesiredDate,
                    StartTime = start,
                    NumberOfHours = NumberOfHours,
                    TotalCost = 0,
                    Paid = 0,
                    BookingStatus = BookingStatus.Confirmed,
                    BookingType = BookingType.Individual,
                };
                await _bookingService.AddAsync(Booking);

                /*Create Payment*/
                var payment = new Payment
                {
                    Status = PaymentStatus.Pending,
                    TotalCost = 0,
                    PaidAmount = 0,
                    LastUpdate = DateTime.Now,
                    BookingId = Booking.Id,

                };
                await _paymentSerivce.AddAsync(payment);
                /*Add HallBooking*/
                /*Get Booked hall*/
                var bookedHall = await _hallSerivce.GetByIdAsync(viewModel.HallId);
                var BookingHall = new BookingHall
                {
                    NumberOfUnits = 1,
                    PricePerUnit = bookedHall.CostPerHour,
                    HallId = bookedHall.Id,
                    BookingId = Booking.Id,
                };
                await _bookingHall_service.AddAsync(BookingHall);

                var paymentHallDetail = new PaymentDetail
                {
                    TotalCost = bookedHall.CostPerHour * NumberOfHours,
                    Status = PaymentStatus.Pending,
                    Type = PaymentType.Cash,
                    PaymentId = payment.Id,
                    BookingHallId = BookingHall.Id,

                };
                await _paymentDetailService.AddAsync(paymentHallDetail);
                Booking.TotalCost += bookedHall.CostPerHour * NumberOfHours;

                /*Loop over services*/
                List<PaymentDetail> paymentServiceDetails = new List<PaymentDetail>();
                foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
                {
                    if (ServiceQuantity.Value > 0)
                    {
                        var service = await _spaceService_Service.GetByIdAsync(ServiceQuantity.Key);
                        var bookingHallService = new BookingHallService()
                        {
                            BookingHallId = BookingHall.Id,
                            ServiceId = service.Id,
                            NumberOfUnits = ServiceQuantity.Value,
                            PricePerUnit = service.Price
                        };
                        await _bookingService_Serivce.AddAsync(bookingHallService);

                        paymentServiceDetails.Add(new PaymentDetail
                        {
                            TotalCost = service.Price * ServiceQuantity.Value,
                            Status = PaymentStatus.Pending,
                            Type = PaymentType.Cash,
                            BookingHallServiceId = bookingHallService.Id
                        });
                        Booking.TotalCost += service.Price * ServiceQuantity.Value;
                    }
                }
                payment.TotalCost = Booking.TotalCost;
                await _bookingService.UpdateAsync(Booking.Id, Booking);
                await _paymentSerivce.UpdateAsync(payment.Id, payment);
            }catch(Exception err)
            {
                ModelState.AddModelError("DesiredDate", "Unexpected error,Try again.");
                return View(IndexModel);
            }
            return RedirectToAction("Index","Explore");
        }
       
    }
}
