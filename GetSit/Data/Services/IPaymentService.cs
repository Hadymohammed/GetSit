using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IPaymentService:IEntityBaseRepository<Payment>
    {
        public Payment GetByBookingId(int bookingId);
        public Payment GetByCustomerBookingId(int bookingId);

    }
}
