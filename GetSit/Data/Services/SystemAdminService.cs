using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SystemAdminService:EntityBaseRepository<SystemAdmin>,ISystemAdminService
    {
        private readonly AppDBcontext _context;
        public SystemAdminService(AppDBcontext context):base(context)
        {
            _context = context;
        }

        public SystemAdmin? GetByEmail(string email)
        {
            return _context.SystemAdmin.Where(a => a.Email == email).FirstOrDefault();
        }
    }
}
