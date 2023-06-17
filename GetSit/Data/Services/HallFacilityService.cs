using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class HallFacilityService:EntityBaseRepository<HallFacility>, IHallFacilityService
    {
        AppDBcontext _context;
        public HallFacilityService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<HallFacility> GetByHallId(int hallId)
        {
            return _context.HallFacility.Where(p => p.HallId == hallId).ToList();
        }
    }
}
