using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class PaymentCardService:EntityBaseRepository<PaymentCard>,IPaymentCardService
    {
        public PaymentCardService(AppDBcontext context):base(context)
        {

        }
    }
}
