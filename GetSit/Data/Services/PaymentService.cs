using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class PaymentService:EntityBaseRepository<Payment>,IPaymentService
    {
        public PaymentService(AppDBcontext context):base(context)
        {

        }
    }
}
