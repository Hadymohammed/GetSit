using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public class PaymentDetailService:EntityBaseRepository<PaymentDetail>,IPaymentDetailService
    {
        public PaymentDetailService(AppDBcontext context):base(context)
        {

        }
    }
}
