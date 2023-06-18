using GetSit.Data.Base;
using GetSit.Models;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface IHallRequestService:IEntityBaseRepository<HallRequest>
    {

        void AddRequest(HallRequest request);
        void DeleteRequest(HallRequest request);
        void UpdateRequest(HallRequest request);
        HallRequest GetById(int RequestId);
        List<HallRequest> GetPendingBySpaceId(int spaceId, params Expression<Func<HallRequest, object>>[] includeProperties);
    }
}
