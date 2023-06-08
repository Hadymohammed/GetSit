using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public class HallRequestService : IHallRequestService
    {
        readonly AppDBcontext _context;
        public HallRequestService(AppDBcontext context) 
        {
            _context = context;
        }
        public void AddRequest(HallRequest request)
        {
            _context.Add(request);
            _context.SaveChanges(); 
        }
        public void DeleteRequest(HallRequest request)
        {
            _context.Remove(request);
            _context.SaveChanges();
        }

        public List<HallRequest> GetPendingBySpaceId(int spaceId, params Expression<Func<HallRequest, object>>[] includeProperties)
        {
            IQueryable<HallRequest> query = _context.Set<HallRequest>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return query.Where(n => n.Hall.SpaceId == spaceId && n.Status != ReqestStatus.Accepted).ToList();
        }
        public HallRequest GetById(int RequestId)
        {
            var ret=_context.HallRequest.Where(h => h.Id == RequestId).Single();
            return ret;
        }

    }
}
