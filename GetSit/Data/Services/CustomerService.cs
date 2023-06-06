using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class CustomerService:EntityBaseRepository<Customer>,ICustomerService
    {
        public CustomerService(AppDBcontext context):base(context)
        {

        }
    }
}
