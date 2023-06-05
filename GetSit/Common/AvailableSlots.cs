using GetSit.Data;
using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;

namespace GetSit.Common
{
    public class AvailableSlots 
    {
        AppDBcontext _context; 
        public AvailableSlots(AppDBcontext context)
        {
            _context = context;
        }
         public List<TimeSpan> GetAvailableSlotsForDay(int hallId, DateTime date)
        {
            var bookings = /*_context.Booking.Include(m=>m.BookingHalls && BookingDate == date).ToList();*/
            _context.Booking
             .Where(b => b.BookingHalls.Any(h => h.Id == hallId && b.BookingDate == date))
             .ToList();

            var space = _context.Space.Where(s => s.Halls.Any(h => h.Id == hallId)).FirstOrDefault();

            /* DayOfWeek dayOfWeek = date.DayOfWeek;

             SpaceWorkingDay? workingDay = _context.SpaceWorkingDay.FirstOrDefault(wd => wd.SpaceId == space.Id && wd.Day == (WeekDay)dayOfWeek);
            */

            // check if the day is out of space working days

            WeekDay currentDay = (WeekDay)date.DayOfWeek;

            SpaceWorkingDay? obj = _context.SpaceWorkingDay.Where(h => h.SpaceId == space.Id && h.Day == currentDay).FirstOrDefault();

            if (obj == null)
            {
                // Add a message indicating that the hall is closed on the current day
               return (new List<TimeSpan> { TimeSpan.MinValue });
            }

            TimeSpan startTime = TimeSpan.FromHours(obj.OpeningTime.TotalHours); // Start time for the day
            TimeSpan endTime = TimeSpan.FromHours(obj.ClosingTime.TotalHours); // End time for the day
            TimeSpan timeSlot = TimeSpan.FromMinutes(15); // Length of each time slot
            var timeSlots = new List<TimeSpan>();

            for (var time = startTime; time < endTime; time += timeSlot)
            {
                timeSlots.Add(time);
            }
            var availableTimeSlots = new List<TimeSpan>();
            foreach (TimeSpan TimeSlot in timeSlots)
            {
                var endTimeSlot = TimeSlot.Add(TimeSlot); // End time for the time slot
                                                          // Check if the time slot is available
                var isAvailable = true;
                foreach (var booking in bookings)
                {
                    var EndTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours));
                    if (booking.StartTime < endTimeSlot && EndTime > TimeSlot)
                    {
                        isAvailable = false;
                        break;
                    }
                }

                // If the time slot is available, add it to the list of available time slots
                if (isAvailable)
                {
                    availableTimeSlots.Add(TimeSlot);
                }

            }
            return availableTimeSlots;
        }

        public List<Dictionary<DateTime, List<TimeSpan>>> GetAvailableSlotsForWeek(int hallId, DateTime date)
        {
            
            List<Dictionary<DateTime,List <TimeSpan>>> availableSlotsForWeek = new List<Dictionary<DateTime, List<TimeSpan>>>() ;
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = date.AddDays(i);
                List<TimeSpan> availableSlots = GetAvailableSlotsForDay(hallId, currentDate);
                Dictionary<DateTime, List<TimeSpan>> DaySlots = new Dictionary<DateTime, List<TimeSpan>>();
                DaySlots.Add(currentDate, availableSlots);
                availableSlotsForWeek.Add(DaySlots);
            }
            return availableSlotsForWeek;
        }
    }
}
