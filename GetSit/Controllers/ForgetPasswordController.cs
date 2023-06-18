using GetSit.Common;
using GetSit.Data.enums;
using GetSit.Data.Security;
using GetSit.Data.Services;
using GetSit.Data.ViewModels;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc;

namespace GetSit.Controllers
{
    public class ForgetPasswordController : Controller
    {
        #region Inject Dependencies
        private readonly IUserManager _userManager;
        private readonly ICustomerService _customerService;
        private readonly ISpaceEmployeeService _providerService;
        private readonly ISystemAdminService _adminService;
        public ForgetPasswordController(IUserManager userManager,
            ICustomerService customerService,
            ISpaceEmployeeService spaceEmployeeService,
            ISystemAdminService systemAdminService)
        {
            _userManager = userManager;
            _customerService = customerService;
            _providerService=spaceEmployeeService;
            _adminService=systemAdminService;
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(ForgetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            switch (vm.Role)
            {
                case UserRole.Admin:
                    var admin=_adminService.GetByEmail(vm.Email);
                    if (admin == null ||admin.Registerd== false)
                    {
                        ModelState.AddModelError("Email", "This email not registered. Do you choose the wrong role?");
                        return View(vm);
                    }
                    SessoinHelper.saveObject(HttpContext,SessoinHelper.ForgetPasswordKey,vm);
                    return RedirectToAction("EmailOTP", "ForgetPassword");
                case UserRole.Customer:
                    var customer = _customerService.GetByEmail(vm.Email);
                    if (customer == null)
                    {
                        ModelState.AddModelError("Email", "This email not registered. Do you choose the wrong role?");
                        return View(vm);
                    }
                    SessoinHelper.saveObject(HttpContext, SessoinHelper.ForgetPasswordKey, vm);
                    return RedirectToAction("EmailOTP", "ForgetPassword");
                case UserRole.Provider:
                    var provider = _providerService.GetByEmail(vm.Email);
                    if (provider == null ||provider.Registerd==false)
                    {
                        ModelState.AddModelError("Email", "This email not registered. Do you choose the wrong role?");
                        return View(vm);
                    }
                    SessoinHelper.saveObject(HttpContext,SessoinHelper.ForgetPasswordKey, vm);
                    return RedirectToAction("EmailOTP","ForgetPassword");
                default:
                    ModelState.AddModelError("Role", "invalid role.");
                    return View(vm);
            }
        }
        [HttpGet]
        public IActionResult EmailOTP()
        {
            ForgetPasswordVM vm=SessoinHelper.getObject<ForgetPasswordVM>(HttpContext, SessoinHelper.ForgetPasswordKey);

            if (vm == null)
                return Redirect("Index");
            OTPServices.SendEmailOTP(HttpContext, vm.Email);
            return View(new OTPVM()
            {
                Email=vm.Email
            });
        }
        [HttpPost]
        public IActionResult EmailOTP(OTPVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if(!OTPServices.VerifyOTP(HttpContext, vm))
            {
                ModelState.AddModelError("OTP", "Invalid OTP.");
                return View(vm);
            }
            return Redirect("AddNewPassword");
        }
        [HttpGet]
        public IActionResult AddNewPassword()
        {
            var forgetPassword = SessoinHelper.getObject<ForgetPasswordVM>(HttpContext, SessoinHelper.ForgetPasswordKey);
            if (forgetPassword == null || forgetPassword.Role == 0)
                return Redirect("Index");
            return View(forgetPassword);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewPassword(ForgetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (vm.Password == null)
            {
                ModelState.AddModelError("Password", "Password is required.");
                return View(vm);
            }
            switch (vm.Role)
            {
                case UserRole.Admin:
                    var admin=_adminService.GetByEmail(vm.Email);
                    admin.Password = PasswordHashing.Encode(vm.Password);
                    await _adminService.UpdateAsync(admin.Id,admin);
                    await _userManager.SignIn(HttpContext, admin);
                    return RedirectToAction("AdminProfile", "Account");
                case UserRole.Customer:
                    var customer = _customerService.GetByEmail(vm.Email);
                    customer.Password = PasswordHashing.Encode(vm.Password);
                    await _customerService.UpdateAsync(customer.Id,customer);
                    await _userManager.SignIn(HttpContext, customer);
                    return RedirectToAction("CustomerProfile", "Account");
                case UserRole.Provider:
                    var provider = _providerService.GetByEmail(vm.Email);
                    provider.Password = PasswordHashing.Encode(vm.Password);
                    await _providerService.UpdateAsync(provider.Id, provider);
                    await _userManager.SignIn(HttpContext, provider);
                    return RedirectToAction("ProviderProfile", "Account");
                default:
                    return Redirect("Index");
            }
        }

    }
}
