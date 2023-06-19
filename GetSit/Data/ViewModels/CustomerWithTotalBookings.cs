using GetSit.Models;

namespace GetSit.Data.ViewModels
{
    public class CustomerWithTotalBookings
    {
        public Customer Customer { get; set; }
        public int TotalBookings { get; set; }
    }
}