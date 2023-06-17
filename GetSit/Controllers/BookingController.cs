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
        [HttpGet]
        public async Task<IActionResult> GetBookingDetails(int ID)
        {
            Booking booking = (Booking)_context.Booking.Include(i => i.BookingHalls).ThenInclude(i => i.BookedServices)
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

            TimeSpan endTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours));

            /* create object from the class to get the available timeslots*/
            AvailableSlots slots = new AvailableSlots(_context);

            var filterDate = booking.DesiredDate;

            var customer = _context.Customer.Where(i => i.Id == booking.CustomerId).FirstOrDefault();

            var userbooking = new BookingVM
            {
                SelectedHall = hall,
                SelectedSpace = space,
                BookingDate = booking.BookingDate,
                DesiredDate = booking.DesiredDate,
                StartTime = booking.StartTime.ToString("hh:mm tt"),
                EndTime = endTime.ToString("hh:mm tt"),
                Paid = booking.Paid,
                TotalCost = booking.TotalCost,
                SelectedServicesQuantities = SelectedServices,
                paymentDetails = paymentDetails,
                FilterDate = filterDate,
                AvailableSlots = slots.GetAvailableSlotsForDay(hall.Id, filterDate),
                SlotsForWeek = slots.GetAvailableSlotsForWeek(hall.Id, filterDate),
                ServicesStatus = ServicesStatus,
                Customer = customer
            };
            return View(userbooking);
        }
        /*
        [HttpPost]
        public async Task<IActionResult> EditBookingByCustomer(int ID, BookingVM viewModel)
        {
            var Booking = _context.Booking.FirstOrDefault(b => b.Id == ID);

            var hall = _context.SpaceHall
               .Where(i => i.Id == Booking.BookingHalls.First().Id).FirstOrDefault();

            var space = _context.Space
                .Where(s => s.Halls.Any(h => h.Id == hall.Id)).FirstOrDefault();

            /// create object from the class to get the available timeslots
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

            //Send unavailable error to the employee
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
        */
        [HttpPost]
        public async Task<IActionResult> EditBookingByEmployee(int ID, BookingVM viewModel)
        {
            foreach (KeyValuePair<int, int> ServiceQuantity in viewModel.SelectedServicesQuantities)
            {
                SpaceService service = _context.SpaceService.Where(s => s.Id == ServiceQuantity.Key).FirstOrDefault();

                var booked = _context.BookingHallService.Where(i => i.ServiceId == service.Id &&
                    i.BookingHallId == viewModel.SelectedHall.Id).FirstOrDefault();

                if (booked != null)
                {
                    var detail = _context.PaymentDetail.Where(i => i.BookingHallServiceId == booked.Id
                        ).FirstOrDefault();

                    // update status  
                    detail.Status = viewModel.ServicesStatus[service.Id];

                }

            }
            _context.SaveChanges();
            return RedirectToAction("GetBookingDetails", "SpaceManagement");
        }

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
