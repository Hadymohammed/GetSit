using GetSit.Data.Base;
using GetSit.Data.enums;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class SpaceContactService : EntityBaseRepository<SpaceContact>, ISpaceContactService
    {
        private readonly AppDBcontext _context;

        public SpaceContactService(AppDBcontext context) : base(context)
        {
        }
    }
}