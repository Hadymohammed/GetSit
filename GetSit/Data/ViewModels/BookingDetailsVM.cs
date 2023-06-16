using GetSit.Models;

namespace GetSit.Data.ViewModels
{
    public class BookingDetailsVM
    {
        public SpaceEmployee Employee { get; set; }
        public Customer? customer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public PaymentDetail? HallDetail { get; set; }
        public Space? Space { get; set; }
        public Booking? CustomerBooking { get; set; }
        public GuestBooking? Booking { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime DesiredDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public float TotalCost { get; set; }
        public List<PaymentDetail>? servicesDetails { get; set; }
        public float? Paid { get; set; }
        public List<TimeSpan>? EndSlots { get; set; }
        public List<SpaceService>? SpaceServices { get; set; }
        public Dictionary<int, int>? SelectedServicesQuantities { get; set; }

    }
}
