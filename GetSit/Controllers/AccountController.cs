using GetSit.Data.ViewModels;
using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using GetSit.Data;
using Microsoft.Win32;
using GetSit.Data.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using GetSit.Common;

namespace GetSit.Controllers
{
    public class AccountController : Controller
    {
        AppDBcontext _context;
        private IUserManager _userManager;

        /* create an object from PasswordHashing class to encode and decode*/
        PasswordHashing hash = new PasswordHashing();
        /* check if the entered password while logging in matches the stored password in database*/
        bool VerifyPassword(string encodedPassword, string password)
        {

            return (hash.Decode (encodedPassword) == password);
        }

        public AccountController(AppDBcontext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            switch (login.Role)
            {
                case UserRole.Admin:
                    var admin = _context.SystemAdmin.Where(c => c.Email == login.Email).FirstOrDefault();/*Varify user : macthed email and password*/
                    if (admin == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email");
                        return View();
                    }
                    if (!VerifyPassword(admin.Password, login.Password))
                    {
                        // Password is incorrect
                        ModelState.AddModelError(string.Empty, "Invalid login attempt, incorrect password.");
                        return View();
                    }

                    break;
                case UserRole.Provider:
                    var provider = _context.SpaceEmployee.Where(c => c.Email == login.Email).FirstOrDefault();/*Varify user : macthed email and password*/
                    if (provider == null)
                    {
                        ModelState.AddModelError(string.Empty, "invalid email");
                        return View();
                    }
                    if (!VerifyPassword(provider.Password, login.Password))
                    {
                        // Password is incorrect
                        ModelState.AddModelError(string.Empty, "Invalid login attempt, incorrect password.");
                        return View();
                    }
              
                    break;
                case UserRole.Customer:
        
                    var customer = _context.Customer.Where(c=>c.Email==login.Email).FirstOrDefault();/*Varify user : macthed email and password*/
                    if (customer == null)
                    {
                        ModelState.AddModelError(string.Empty, "invalid email");
                        return View();
                    }
                    if (!VerifyPassword(customer.Password, login.Password))
                    {
                        // Password is incorrect
                        ModelState.AddModelError(string.Empty, "Invalid login attempt, incorrect password.");
                        return View();
                    }
                    /*Get DB user*/
                    _userManager.SignIn(HttpContext, customer);
                    return RedirectToAction("CustomerProfile", "Account");
                    /*context sign in */
                    break;
                default:
                    break;
            }
            return View();
        }
        [HttpPost]
        public IActionResult logout()
        {
            _userManager.SignOut(HttpContext);
            return RedirectToAction("Login");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            

            switch (register.Role)
            {
                case UserRole.Admin:
                    var admin = new SystemAdmin()
                    {
                       
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        Birthdate = register.Birthdate,
                        Password = hash.Encode (register.Password),/*Here password should be hashed*/
                    };
                    try
                    {
                    await _context.SystemAdmin.AddAsync(admin);
                    _context.SaveChanges();
                    await _userManager.SignIn(HttpContext, admin);
                    return RedirectToAction("AdminProfile", "Account");
                    }
                    catch (Exception error)
                    {
                        return View(register);
                    }
                    break;
                case UserRole.Provider:
                    var provider = new SpaceEmployee()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        Birthdate = register.Birthdate,
                        Password = hash.Encode(register.Password),/*Here password should be hashed*/
                    };
                    try
                    {
                    await _context.SpaceEmployee.AddAsync(provider);
                    _context.SaveChanges();
                    await _userManager.SignIn(HttpContext, provider);
                    return RedirectToAction("ProviderProfile", "Account");
                    }
                    catch (Exception error)
                    {
                        return View(register);
                    }
                    break;
                case UserRole.Customer:
                    var customer = new Customer()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        CustomerType=CustomerType.Registered,
                        Birthdate=register.Birthdate,
                        Password = hash.Encode(register.Password),/*Here password should be hashed*/
                    };
                    try
                    {
                       await _context.Customer.AddAsync(customer);
                        _context.SaveChanges();
                       await _userManager.SignIn(HttpContext, customer);
                       return RedirectToAction("CustomerProfile");
                    }
                    catch (Exception error)
                    {
                        return View(register);
                    }
                    break;
                default:
                    return View(register);
                    break;
            }
            return View(register);
        }
        [Authorize(Roles = "Admin")]//error enum must be used
        public IActionResult AdminProfile()
        {
            var user = _userManager.GetCurrentUser(HttpContext);
            return View(user);
        }
        [Authorize(Roles = "Customer")]//error enum must be used
        public IActionResult CustomerProfile()
        {
            var user = _userManager.GetCurrentUser(HttpContext);
            return View(user);
        }
        [Authorize(Roles = "Provider")]//error enum must be used
        public IActionResult ProviderProfile()
        {
            var user = _userManager.GetCurrentUser(HttpContext);
            return View(user);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
