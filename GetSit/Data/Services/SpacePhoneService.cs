using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{

    public class SpacePhoneService : EntityBaseRepository<SpacePhone>, ISpacePhoneService
    {
        private readonly AppDBcontext _context;
        public SpacePhoneService(AppDBcontext context) : base(context)
        {

        }
        public SystemAdmin GetByPhoneNumber(string PhoneNumber)
        {
            return _context.SystemAdmin.Where(a => a.PhoneNumber == PhoneNumber).FirstOrDefault();
        }
    }
}