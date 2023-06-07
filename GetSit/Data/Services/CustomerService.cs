using GetSit.Data.Base;
using GetSit.Models;

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
    }
}
