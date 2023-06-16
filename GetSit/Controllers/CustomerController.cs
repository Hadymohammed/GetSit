using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data;
using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using GetSit.Models;

namespace GetSit.Controllers
{
    [Authorize(policy: "CustomerPolicy")]
    public class CustomerController : Controller
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
        private readonly ICustomerService _customerService;


        public CustomerController(AppDBcontext context,
            IUserManager userManager,
            IBookingService bookingService,
            ISpaceHallService spaceHallService,
            IBookingHall_Service bookingHall_service,
            ISpaceService_Service spaceService_Service,
            IBookingHallService_Service bookingService_Serivce,
            IPaymentService paymentService,
            IPaymentDetailService paymentDetailService,
            ISpaceService spaceService,
            ICustomerService customerService
            )
            
        {
            _context = context;
            _userManager = userManager;
            _bookingService = bookingService;
            _hallSerivce = spaceHallService;
            _bookingHall_service = bookingHall_service;
            _spaceService_Service = spaceService_Service;
            _bookingService_Serivce = bookingService_Serivce;
            _paymentSerivce = paymentService;
            _paymentDetailService = paymentDetailService;
            _spaceService = spaceService;
            _customerService = customerService; 
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            int customerId = _userManager.GetCurrentUserId(HttpContext);
            var customer =  _customerService.GetById(customerId);
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            int customerId = _userManager.GetCurrentUserId(HttpContext);
            var customer = await _customerService.GetByIdAsync(customerId);
            var model = new EditCustomerProfileVM
            {
                Password = customer.Password,
                Country = customer.Country,
                City = customer.City,
                ProfilePictureUrl = customer.ProfilePictureUrl,
                PhoneNumber = customer.PhoneNumber,
                Name = customer.FirstName + " " + customer.LastName,
                Title = customer.Title,
                 Faculty = customer.Faculty,
                
            };
            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditCustomerProfileVM EditModel)
        {
            int customerId = _userManager.GetCurrentUserId(HttpContext);
            if (ModelState.IsValid)
            {
                // Get the customer from the database
                var customer = await _customerService.GetByIdAsync(customerId);

                // Map the properties from the EditCustomerViewModel to the Customer model
                customer.Password = EditModel.Password;
                customer.Country = EditModel.Country;
                customer.City = EditModel.City;
                customer.PhoneNumber = EditModel.PhoneNumber;
                customer.Faculty = EditModel.Faculty;

                // Update the customer in the database
                await _customerService.UpdateAsync(customerId,customer);

                // Redirect to the Customers action method
                return RedirectToAction("Index", "Customer");
            }

            // If the model state is not valid, return the Edit view with the EditModel
            return View(EditModel);
        }
    }
}
