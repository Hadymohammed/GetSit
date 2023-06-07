using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISystemAdminService:IEntityBaseRepository<SystemAdmin>
    {
        public SystemAdmin GetByEmail(string email);
    }
}
