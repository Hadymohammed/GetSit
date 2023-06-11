using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ISpacePhoneService : IEntityBaseRepository<SpacePhone>
    {
        public SystemAdmin GetByPhoneNumber(string PhoneNumber);
    }
}
