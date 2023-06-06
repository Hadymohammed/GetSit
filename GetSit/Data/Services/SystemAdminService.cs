using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SystemAdminService:EntityBaseRepository<SystemAdmin>,ISystemAdminService
    {
        public SystemAdminService(AppDBcontext context):base(context)
        {

        }
    }
}
