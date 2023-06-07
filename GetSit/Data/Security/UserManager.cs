using GetSit.Data.enums;
using GetSit.Data.Services;
using GetSit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;

namespace GetSit.Data.Security
{
    public class UserManager : IUserManager
    {
        private readonly AppDBcontext _context;
        private readonly ICustomerService _customerService;
        private readonly ISpaceEmployeeService _spaceEmployeeService;
        private readonly ISystemAdminService _adminSerivce;
        public UserManager(AppDBcontext context,ICustomerService customerService,ISpaceEmployeeService spaceEmployeeService,ISystemAdminService systemAdminService)
        {
            _context = context;
            _customerService = customerService;
            _spaceEmployeeService = spaceEmployeeService;
            _adminSerivce = systemAdminService;
        }

        public async Task<IAbstractUser> GetCurrentUserAsync(HttpContext httpContext)
        {
            int userId = this.GetCurrentUserId(httpContext);
            string userRole = this.GetUserRole(httpContext);

            if (userId == -1)
                return null;
            if (userRole is null)
                return null;

            IAbstractUser user;
            if (userRole == UserRole.Customer.ToString())
            {
                user = await _customerService.GetByIdAsync(userId);
            }
            else if (userRole == UserRole.Admin.ToString())
            {
                user = await _adminSerivce.GetByIdAsync(userId);
            }
            else if (userRole == UserRole.Provider.ToString())
            {
                user = await _spaceEmployeeService.GetByIdAsync(userId);
            }
            else user = null;

            return user;
            }

        public int GetCurrentUserId(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return -1;

            var claim = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (claim == null)
                return -1;
            int currentUserId;

            if (!int.TryParse(claim.Value, out currentUserId))
                return -1;

            return currentUserId;
        }

        public IEnumerable<Claim> GetUserClaims<T>(T user) where T : IAbstractUser
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim("Email", user.Email));

            if(user is Customer)
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRole.Customer.ToString()));
            }
            else if(user is SystemAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRole.Admin.ToString()));

            }
            else if (user is SpaceEmployee)
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRole.Provider.ToString()));

            }
            return claims;
        }

        public string GetUserRole(HttpContext httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
                return null;

            var claim = httpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault();
            if (claim == null)
                return null;
            string currentUserRole=claim.Value;

            return currentUserRole;
        }

        public async Task SignIn<T>(HttpContext httpContext, T user, bool isPersistent = false) where T : IAbstractUser
        {
            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims<T>(user), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties() { IsPersistent = isPersistent }
            );
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
