using GetSit.Data.Base;
using GetSit.Data.ViewModels;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class Space_Service:EntityBaseRepository<Space>,ISpaceService
    {
        AppDBcontext _context;
        public Space_Service(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public List<CustomerWithTotalBookings> GetCustomersWithMostBookings(int spaceId, int count)
        {

            var result = _context.Booking
            .Where(booking => booking.BookingHalls.Any(bh => bh.Hall.SpaceId == spaceId))
    .GroupBy(booking => booking.Customer)
    .Select(group => new CustomerWithTotalBookings
    {
        Customer = group.Key,
        TotalBookings = group.Count()
    })
    .OrderByDescending(customer => customer.TotalBookings)
    .Take(count)
    .ToList();

            return result;

        }

    }
}
