using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class BookingHallService_Service:EntityBaseRepository<BookingHallService>,IBookingHallService_Service
    {
        public BookingHallService_Service(AppDBcontext context):base(context)
        {

        }
    }
}
