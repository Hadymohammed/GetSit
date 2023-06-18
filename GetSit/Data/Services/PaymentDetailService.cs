using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class PaymentDetailService:EntityBaseRepository<PaymentDetail>,IPaymentDetailService
    {
        readonly AppDBcontext _context;
        public PaymentDetailService(AppDBcontext context):base(context)
        {
            _context=context;
        }

        public List<PaymentDetail> GetByPaymendId(int paymendId, params Expression<Func<PaymentDetail, object>>[] includeProperties)
        {
            IQueryable<PaymentDetail> query = _context.Set<PaymentDetail>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(n => n.PaymentId == paymendId).ToList();
        }
    }
}
