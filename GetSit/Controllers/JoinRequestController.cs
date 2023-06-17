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
using GetSit.Data.Services;
using Newtonsoft.Json;

namespace GetSit.Controllers
{
    public class JoinRequestController : Controller
    {
        #region Dependencies
        private readonly AppDBcontext _context;
        private readonly IUserManager _userManager;
        private readonly ISpaceEmployeeService _providerService;
        private readonly ISpaceService _spaceService;
        public JoinRequestController(AppDBcontext context,
            IUserManager userManager,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService)
        {
            _context = context;
            _userManager = userManager;
            _providerService = spaceEmployeeService;
            _spaceService = spaceService;
        }
        #endregion
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ProviderRegisterVM viewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(viewModel);
            }
            if(_providerService.GetByEmail(viewModel.Email) != null)
            {
                ModelState.AddModelError("Email", "This email already has an account.");
                return View(viewModel);
            }

            string password= RandomPassword.GenerateRandomPassword(8); 
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
            HttpContext.Session.SetString("RegisterModel", JsonConvert.SerializeObject(Manager));

            var otpVm = new OTPVM()
            {
                Email=viewModel.Email,
                Phone=viewModel.PhoneNumber
            };
            return RedirectToAction("PhoneOTP", otpVm);
        }
        #region OTP verfication
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
            return RedirectToAction("SpaceDetails");
        }
        #endregion
        
        #region Space info
        [HttpGet]
        public IActionResult SpaceDetails()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SpaceDetails(AddSpaceVM spaceVM)
        {
            if (!ModelState.IsValid)
            {
                return View(spaceVM);
            }
            var stringUser = HttpContext.Session.GetString("RegisterModel");
            if (stringUser == null)
            {
                return RedirectToAction("Index");
            }
            var manager = JsonConvert.DeserializeObject<SpaceEmployee>(stringUser) as SpaceEmployee;

            var space = new Space
            {
                Name = spaceVM.SpaceName,
                Bio = spaceVM.SpaceBio,
                Country = spaceVM.SpaceCountry,
                City = spaceVM.SpaceCity,
                Street = spaceVM.SpaceStreet,
                GPSLocation = spaceVM.SpaceGPSLocation,
                IsFast = true,
                IsApproved = false,
                BankAccount="None",
                DateCreated = DateTime.Today,
            };
            await _spaceService.AddAsync(space);
            manager.SpaceId = space.Id;
            await _providerService.AddAsync(manager);

            return RedirectToAction("Success",new JoinRequestVM()
            {
                FirstName=manager.FirstName,
                LastName=manager.LastName,
                Email=manager.Email,
                PhoneNumber=manager.PhoneNumber,
                Birthdate=manager.Birthdate,
                SpaceName=space.Name,
                SpaceBio=space.Bio,
                SpaceCountry=space.Country,
                SpaceCity=space.City,
                SpaceStreet=space.Street,
                SpaceGPSLocation=space.GPSLocation
            });
        }
        #endregion Space info
        [HttpGet]
        public IActionResult Success(JoinRequestVM vm)
        {
            if (vm == null)
                return NotFound();
            return View(vm);
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
            Space.DateCreated = DateTime.Today;
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
