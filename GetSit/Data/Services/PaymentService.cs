using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class PaymentService:EntityBaseRepository<Payment>,IPaymentService
    {
        AppDBcontext _context;
        public PaymentService(AppDBcontext context):base(context)
        {
            _context = context;
        }
        public Payment GetByBookingId(int bookingId)
        {
            return _context.Payment.Where(b => b.GuestBookingId == bookingId)
                    .Include(b => b.Booking)
                        .ThenInclude(bo => bo.BookingHalls)
                            .ThenInclude(bh => bh.BookedServices)
                    .Include(b=>b.Booking)
                        .ThenInclude(bo=>bo.Customer)
                    .Include(b => b.Details)
                    .FirstOrDefault();
        }
    }
}
