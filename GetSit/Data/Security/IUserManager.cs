using System.Security.Claims;

namespace GetSit.Data.Security
{
    public interface IUserManager
    {
        Task SignIn<T>(HttpContext httpContext, T user, bool isPersistent = false) where T : IAbstractUser;
        Task SignOut(HttpContext httpContext);
        int GetCurrentUserId(HttpContext httpContext);
        Task<IAbstractUser> GetCurrentUserAsync(HttpContext httpContext);
        IEnumerable<Claim> GetUserClaims<T>(T user) where T : IAbstractUser;
        public string GetUserRole(HttpContext httpContext);
    }
}
