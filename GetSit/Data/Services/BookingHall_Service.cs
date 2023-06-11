using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class BookingHall_Service:EntityBaseRepository<BookingHall>,IBookingHall_Service
    {
        public BookingHall_Service(AppDBcontext context):base(context)
        {

        }
    }
}
