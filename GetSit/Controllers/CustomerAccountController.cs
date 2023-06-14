using GetSit.Data;
using GetSit.Data.Services;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using GetSit.Data.Security;
using GetSit.Common;
using System;

namespace GetSit.Controllers
{
    public class CustomerAccountController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        private readonly ICustomerService _customerService;


        public CustomerAccountController(AppDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index(int Id)
        {
            var customer = _context.Customer.Include(c => c.Bookings)
                .Where(m => m.Id == Id).FirstOrDefault();
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> EditCustomerProfile(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            // Create an instance of the view model and populate it with the user's current data
            var model = new EditCustomerProfileViewModel
            {
                Password = customer.Password,
                Country = customer.Country,
                City = customer.City,
                ProfilePictureUrl = customer.ProfilePictureUrl,
                PhoneNumber = customer.PhoneNumber,
            };
            return View(model);
        }
        
        [HttpPost]
        
        public async Task<IActionResult> Edit(int id,EditCustomerProfileViewModel editCustomerViewModel)
        {
            if (ModelState.IsValid)
            {
                // Get the customer from the database
                var customer = await _customerService.GetByIdAsync(id);

                // Map the properties from the EditCustomerViewModel to the Customer model
                customer.Password = editCustomerViewModel.Password;
                customer.Country = editCustomerViewModel.Country;
                customer.City = editCustomerViewModel.City;
                customer.ProfilePictureUrl = editCustomerViewModel.ProfilePictureUrl;
                customer.PhoneNumber = editCustomerViewModel.PhoneNumber;

                // Update the customer in the database
                await _customerService.UpdateAsync(id,customer);

                // Redirect to the Customers action method
                return RedirectToAction("Index", "Customer");
            }

            // If the model state is not valid, return the Edit view with the EditCustomerViewModel
            return View(editCustomerViewModel);
        }
    }
}
