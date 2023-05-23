using GetSit.Data.ViewModels;
using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using GetSit.Data;
using Microsoft.Win32;
using GetSit.Data.Security;
using Microsoft.AspNetCore.Authorization;

namespace GetSit.Controllers
{
    public class AccountController : Controller
    {
        AppDBcontext _context;
        private IUserManager _userManager;
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
                    var admin = new SystemAdmin()
                    {
                        Email = login.Email,
                        Password = login.Password,
                    };
                    /*Varify user : macthed email and password*/
                    /*Get DB user*/
                    /*context sign in */
                    break;
                case UserRole.Provider:
                    var provider = new SpaceEmployee()
                    {
                        Email = login.Email,
                        Password = login.Password,/*Here password should be decrypted*/
                    };
                    /*Varify user : macthed email and password*/
                    /*Get DB user*/
                    /*context sign in */
                    break;
                case UserRole.Customer:
        
                    var user = _context.Customer.Where(c=>c.Email==login.Email&&c.Password==login.Password).FirstOrDefault();/*Varify user : macthed email and password*/
                    /*Get DB user*/
                    _userManager.SignIn(HttpContext, user);
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
                        Password = register.Password,/*Here password should be hashed*/
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
                        Password = register.Password,/*Here password should be hashed*/
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
                        Password = register.Password,/*Here password should be hashed*/
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
