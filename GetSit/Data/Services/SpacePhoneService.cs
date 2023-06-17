using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class SpacePhoneService:EntityBaseRepository<SpacePhone>,ISpacePhoneService
    {
        readonly AppDBcontext _context;
        public SpacePhoneService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<SpacePhone> GetBySpaceId(int spaceId)
        {
            return _context.SpacePhone.Where(s => s.SpaceId == spaceId).ToList();
        }

    }
}
