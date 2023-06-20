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
        private readonly ICustomerService _customerService;
        private readonly ISystemAdminService _adminService;
        public JoinRequestController(AppDBcontext context,
            IUserManager userManager,
            ISpaceEmployeeService spaceEmployeeService,
            ISpaceService spaceService,
            ICustomerService customerService,
            ISystemAdminService adminService)
        {
            _context = context;
            _userManager = userManager;
            _providerService = spaceEmployeeService;
            _spaceService = spaceService;
            _customerService = customerService;
            _adminService = adminService;
        }
        #endregion
        bool PresirvedEmail(string email)
        {
            return (_customerService.GetByEmail(email) != null ||
                    _providerService.GetByEmail(email) != null ||
                    _adminService.GetByEmail(email) != null
                    );
        }
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
            if(PresirvedEmail(viewModel.Email))
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
                ProfilePictureUrl= Consts.userProfilePhotoHolder,
                IsApproved = false,
            };
            HttpContext.Session.SetString("RegisterModel", JsonConvert.SerializeObject(Manager));

            var otpVm = new OTPVM()
            {
                Email=viewModel.Email,
                Phone=viewModel.PhoneNumber
            };
            return RedirectToAction("EmailOTP", otpVm);//Skip Phone otp
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
                JoinRequestDate = DateTime.Now,
                SpaceCover=Consts.SpaceCoverHolder,
                SpaceLogo=Consts.SpaceLogoHolder
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

    }
}
