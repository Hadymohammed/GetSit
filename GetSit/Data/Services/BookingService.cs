using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class BookingService: EntityBaseRepository<Booking>, IBookingService
    {
        readonly AppDBcontext _context;
        public BookingService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<Booking> GetBySpaceId(int spaceId)
        {
            return _context.Booking
                    .Where(b => b.BookingHalls.Any(bh => bh.Hall.SpaceId == spaceId))
                    .Include(b => b.Customer)
                    .Include(b => b.BookingHalls)
                        .ThenInclude(bh => bh.Hall)
                    .OrderBy(b => b.DesiredDate)
                    .ThenBy(b => b.StartTime)
                    .ToList();
        }
    }
}
