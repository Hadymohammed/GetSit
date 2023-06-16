using GetSit.Data.Base;
using GetSit.Models;

using Microsoft.EntityFrameworkCore;

namespace GetSit.Data.Services
{
    public class CustomerService:EntityBaseRepository<Customer>,ICustomerService
    {
        private readonly AppDBcontext _context;
        public CustomerService(AppDBcontext context):base(context)
        {
            _context = context;
        }

        public Customer? GetByEmail(string email)
        {
            return _context.Customer.Where(c => c.Email == email).FirstOrDefault();
        }
        public Customer GetById(int id)
        {
            var customer= _context.Customer.Where(c=>c.Id == id).
                Include(c => c.Faculty).
                Include(c => c.Bookings).
                ThenInclude(b=>b.BookingHalls).
                ThenInclude(h=>h.Hall).
                ThenInclude(s=>s.Space).
                FirstOrDefault();
            return customer;
        }
    }
}
