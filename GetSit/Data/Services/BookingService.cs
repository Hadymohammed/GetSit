using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class BookingService: EntityBaseRepository<Booking>, IBookingService
    {
        public BookingService(AppDBcontext context):base(context)
        {

        }
    }
}
