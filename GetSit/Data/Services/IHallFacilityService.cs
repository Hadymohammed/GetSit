using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IHallFacilityService:IEntityBaseRepository<HallFacility>
    {
        public List<HallFacility> GetByHallId(int hallId);

    }
}
