using GetSit.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace GetSit.Controllers
{
    public class AccountController : Controller
    {
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
            return View();
        }
    }
}
