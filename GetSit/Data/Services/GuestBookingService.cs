using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class GuestBookingService : EntityBaseRepository<GuestBooking>, IGuestBookingService
    {
        AppDBcontext _context;
        public GuestBookingService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<GuestBooking> GetBySpaceId(int spaceId)
        {
            return _context.GuestBooking
                    .Where(b => b.BookingHalls.Any(bh => bh.Hall.SpaceId == spaceId))
                    .Include(b=>b.Employee)
                    .Include(b => b.BookingHalls)
                        .ThenInclude(bh => bh.Hall)
                    .OrderBy(b => b.DesiredDate)
                    .ThenBy(b => b.StartTime)
                    .ToList();
        }

    }
}
