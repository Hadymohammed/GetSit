using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface ICustomerService:IEntityBaseRepository<Customer>
    {
        public Customer GetByEmail(string email);
    }
}
