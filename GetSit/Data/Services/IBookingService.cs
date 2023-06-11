using GetSit.Data.Base;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GetSit.Data.Services
{
    public interface IBookingService:IEntityBaseRepository<Booking>
    {
        public List<Booking> GetBySpaceId(int spaceId);
    }
}
