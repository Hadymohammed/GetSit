using GetSit.Models;

namespace GetSit.Data.ViewModels
{
    public class SpaceManagementVM
    {
        public Space Space { get; set; }
        public List<SpaceHall> Halls { get; set; }
        public List<SpaceService> Services { get; set; }
        public List<SpaceEmployee> Employees { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<GuestBooking> GuestBookings { get; set; }
    }
}
