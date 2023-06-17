using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface IPaymentDetailService:IEntityBaseRepository<PaymentDetail>
    {
        public List<PaymentDetail> GetByPaymendId(int paymendId, params Expression<Func<PaymentDetail, object>>[] includeProperties);
    }
}
