using GetSit.Data.Security;
using GetSit.Data;
using Microsoft.AspNetCore.Mvc;
using GetSit.Data.ViewModels;
using GetSit.Models;
using GetSit.Common;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using Microsoft.Win32;

namespace GetSit.Controllers
{
    public class JoinRequestController : Controller
    {
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        public JoinRequestController(AppDBcontext context, IUserManager userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RequestToJoin(JoinRequestVM viewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(viewModel);
            }
            RandomPassword rand = new RandomPassword(_context);
            string password = rand.GenerateRandomPassword(8);

            var Manager = new SpaceEmployee
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Password = PasswordHashing.Encode(password),
                Email = viewModel.Email,
                Birthdate = viewModel.Birthdate,
                IsApproved = false,
            };
            var Space = new Space
            {
                Name = viewModel.SpaceName,
                Bio = viewModel.SpaceBio,
                Country = viewModel.SpaceCountry,
                City = viewModel.SpaceCity,
                Street = viewModel.SpaceStreet,
                GPSLocation = viewModel.SpaceGPSLocation,
                IsFast = viewModel.IsFast,
                IsApproved = false,

            };
            Manager.Space = Space;
            Manager.SpaceId = Space.Id;
            await _context.SpaceEmployee.AddAsync(Manager);
            await _context.Space.AddAsync(Space);
            _context.SaveChanges();

            var otpVm = new OTPVM();
            otpVm.Email = viewModel.Email;
            otpVm.Phone = viewModel.PhoneNumber;

            return RedirectToAction("PhoneOTP", otpVm);
        }

        [HttpGet]
        public IActionResult PhoneOTP(OTPVM? otpVm)
        {
            if (otpVm is null)
                RedirectToAction("RequestToJoin");
            OTPServices.SendPhoneOTP(HttpContext, otpVm.Phone);
            return View(otpVm);
        }
        [HttpPost]
        public async Task<IActionResult> PhoneOTPAsync(OTPVM otp)
        {
            if (!ModelState.IsValid)
            {
                return View(otp);
            }
            if (OTPServices.VerifyOTP(HttpContext, otp) == false)
            {
                ModelState.AddModelError("OTP", "InValid Code");
                return View(otp);
            }

            return RedirectToAction("EmailOTP", otp);
        }


        [HttpGet]
        public IActionResult EmailOTP(OTPVM? otpVm)
        {
            if (otpVm is null)
                RedirectToAction("RequestToJoin");
            OTPServices.SendEmailOTP(HttpContext, otpVm.Email);
            return View(otpVm);
        }

        [HttpPost]
        public async Task<IActionResult> EmailOTPAsync(OTPVM otp)
        {
            if (!ModelState.IsValid)
            {
                return View(otp);
            }
            if (OTPServices.VerifyOTP(HttpContext, otp) == false)
            {
                ModelState.AddModelError("OTP", "InValid Code");
                return View(otp);
            }
            return RedirectToAction("RequestToJoin");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRequest(int SpaceID, int ManagerID)
        {
            var Space = _context.Space.Where(i => i.Id == SpaceID).FirstOrDefault();
            var Employee = _context.SpaceEmployee.Where(i => i.Id == ManagerID).FirstOrDefault();

            // Send Acceptance Email with Password
            var fromAddress = new MailAddress("getsit594@gmail.com", "GetSit");
            var toAddress = new MailAddress(Employee.Email, "New User");
            const string fromPassword = "esyqrxcqijyqnpwf";
            const string subject = "GetSit Join Request Acceptance";
            string body = $"Sent from GetSit , you succefully joined Us; " +
                $"Here is your password {PasswordHashing.Decode(Employee.Password)}, " +
                $"You can now use it with your Email to log in." +
                $"To change your password go to Settings in your profile." +
                $"If you have any qustions regarding your upcoming processes do not hesitate to contact us.";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            Space.IsApproved = true;
            Employee.IsApproved = true;
            _context.SaveChanges();

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectRequest(int SpaceID, int ManagerID)
        {
            var Space = _context.Space.Where(i => i.Id == SpaceID).FirstOrDefault();
            var Employee = _context.SpaceEmployee.Where(i => i.Id == ManagerID).FirstOrDefault();

            // Send Rejection Email 
            var fromAddress = new MailAddress("getsit594@gmail.com", "GetSit");
            var toAddress = new MailAddress(Employee.Email, "New User");
            const string fromPassword = "esyqrxcqijyqnpwf";
            const string subject = "GetSit Join Request Rejection";
            string body = $"Sent from GetSit ; " +
                $"We are sorry, The uploaded Data does not match GetSit Criteria." + 
                $"If you have any qustions do not hesitate to contact us.";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

            _context.Space.Remove(Space);
            _context.SpaceEmployee.Remove(Employee);
            _context.SaveChanges();
            return View();
        }

    }
}
