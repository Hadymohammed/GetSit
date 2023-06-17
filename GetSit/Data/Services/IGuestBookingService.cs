using GetSit.Data.Base;
using GetSit.Models;

namespace GetSit.Data.Services
{
    public interface IGuestBookingService:IEntityBaseRepository<GuestBooking>
    {
        public List<GuestBooking> GetBySpaceId(int spaceId);

    }
}
