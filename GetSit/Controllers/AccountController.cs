using GetSit.Data.ViewModels;
using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using GetSit.Data;
using Microsoft.Win32;

namespace GetSit.Controllers
{
    public class AccountController : Controller
    {
        AppDBcontext _context;
        public AccountController(AppDBcontext context)
        {
            _context = context;
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
                    var customer = new Customer()
                    {
                        Email = login.Email,
                        Password = login.Password,/*Here password should be decrypted*/
                    };
                    /*Varify user : macthed email and password*/
                    /*Get DB user*/
                    /*context sign in */
                    break;
                default:
                    break;
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterVM register)
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
                        Password = register.Password,/*Here password should be hashed*/
                    };
                    _context.SystemAdmin.Add(admin);
                    /*context sign in*/
                    break;
                case UserRole.Provider:
                    var provider = new SpaceEmployee()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        Password = register.Password,/*Here password should be hashed*/
                    };
                    _context.SpaceEmployee.Add(provider);
                    /*context sign in*/
                    break;
                case UserRole.Customer:
                    var customer = new Customer()
                    {
                        FirstName = register.FirstName,
                        LastName = register.LastName,
                        Email = register.Email,
                        PhoneNumber = register.PhoneNumber,
                        Password = register.Password,/*Here password should be hashed*/
                    };
                    _context.Customer.Add(customer);
                    /*context sign in*/
                    break;
                default:
                    break;
            }
            return View();
        }
    }
}
