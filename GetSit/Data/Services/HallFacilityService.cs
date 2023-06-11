using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class HallFacilityService:EntityBaseRepository<HallFacility>, IHallFacilityService
    {
        public HallFacilityService(AppDBcontext context):base(context)
        {

        }
    }
}
