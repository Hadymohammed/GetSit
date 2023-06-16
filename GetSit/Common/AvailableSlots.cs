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
         public List<Tuple<TimeSpan,bool>> GetAvailableSlotsForDay(int hallId, DateTime date)
        {
            var bookings = _context.Booking
                           .Where(b => b.BookingHalls.Any(h => h.HallId == hallId && b.DesiredDate == date))
                           .ToList();
            var guestBooking = _context.GuestBooking
                            .Where(b=>b.BookingHalls.Any(h => h.HallId == hallId && b.DesiredDate == date))
                            .ToList();
            var space = _context.Space.Where(s => s.Halls.Any(h => h.Id == hallId)).FirstOrDefault();

            // check if the day is out of space working days

            var currentDay = date.DayOfWeek;

            SpaceWorkingDay? obj = _context.SpaceWorkingDay.Where(h => h.SpaceId == space.Id && h.Day == currentDay).FirstOrDefault();
            //Form the default 96 day time slots
            var timeSlots = new List<Tuple<TimeSpan,bool>>();

            for (TimeSpan time = new TimeSpan(0,0,0); time <= new TimeSpan(24, 0, 0); time += TimeSpan.FromMinutes(15))
            {
                Tuple<TimeSpan,bool> slot = new Tuple<TimeSpan,bool>(time, false);
                timeSlots.Add(slot);
            }
            if (obj == null)
            {
                // all slots are = false
               return timeSlots;
            }
            try
            {
                TimeSpan startTime = TimeSpan.FromHours(obj.OpeningTime.TotalHours); // Start time for the day
                TimeSpan endTime = TimeSpan.FromHours(obj.ClosingTime.TotalHours); // End time for the day
                TimeSpan timeSlot = TimeSpan.FromMinutes(15); // Length of each time slot
                var slotIdx = (int)(startTime.TotalMinutes / 15);

                for (; slotIdx < (int)(endTime.TotalMinutes / 15); slotIdx++)
                {
                    var endTimeSlot = timeSlots[slotIdx].Item1.Add(timeSlot); // End time for the time slot
                                                                              // Check if the time slot is available
                    var isAvailable = true;
                    foreach (var booking in bookings)
                    {
                        var EndTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours));
                        if (booking.StartTime <= timeSlots[slotIdx].Item1 && EndTime >= endTimeSlot)
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                    foreach (var booking in guestBooking)
                    {
                        var EndTime = booking.EndTime;
                        if (booking.StartTime <= timeSlots[slotIdx].Item1 && EndTime >= endTimeSlot)
                        {
                            isAvailable = false;
                            break;
                        }
                    }
                    // If the time slot is available, add it to the list of available time slots
                    if (isAvailable)
                    {
                        timeSlots[slotIdx] = new Tuple<TimeSpan, bool>(TimeSpan.FromMinutes(slotIdx * 15), true);
                    }

                }
            }catch(Exception e)
            {
                return timeSlots;
            }
            return timeSlots;
        }

        public List<Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>> GetAvailableSlotsForWeek(int hallId, DateTime date)
        {
            
            List<Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>> availableSlotsForWeek = new List<Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>>() ;
            for (int i = 0; i < 7; i++)
            {
                DateTime currentDate = date.AddDays(i);
                List<Tuple<TimeSpan,bool>> Slots = GetAvailableSlotsForDay(hallId, currentDate);
                Dictionary<DateTime, List<Tuple<TimeSpan,bool>>> DaySlots = new Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>();
                DaySlots.Add(currentDate, Slots);
                availableSlotsForWeek.Add(DaySlots);
            }
            return availableSlotsForWeek;
        }

        public bool IsTimeSlotAvailable(int hallId, DateTime desiredDate, TimeSpan startTime, TimeSpan endTime)
        {
            TimeSpan duration = endTime - startTime;
            var bookings = _context.Booking
                .Where(b =>
                    b.BookingHalls.Any(bh => bh.HallId == hallId) &&
                    b.DesiredDate.Date == desiredDate.Date)
                .ToList();
            bool NotAvailable = bookings
                .Any(b =>
                    (b.StartTime >= startTime && b.StartTime < endTime) ||
                    (b.StartTime < startTime && b.StartTime.Add(TimeSpan.FromHours(b.NumberOfHours)) > startTime));

            return !NotAvailable;
        }
    
        public List<TimeSpan> GetAvailableEndSlots(int hallId, DateTime date,TimeSpan startTime)
        {
            var bookings = _context.Booking
                           .Where(b => b.BookingHalls.Any(h => h.HallId == hallId && b.DesiredDate == date))
                           .ToList();
            var guestBooking = _context.GuestBooking
                            .Where(b => b.BookingHalls.Any(h => h.HallId == hallId && b.DesiredDate == date))
                            .ToList();
            var space = _context.Space.Where(s => s.Halls.Any(h => h.Id == hallId)).FirstOrDefault();

            // check if the day is out of space working days

            var currentDay = date.DayOfWeek;

            SpaceWorkingDay? obj = _context.SpaceWorkingDay.Where(h => h.SpaceId == space.Id && h.Day == currentDay).FirstOrDefault();
            //Form the default 96 day time slots
            TimeSpan endTime = TimeSpan.FromHours(obj.ClosingTime.TotalHours); // End time for the day
            TimeSpan timeSlot = TimeSpan.FromMinutes(15); // Length of each time slot
            var timeSlots = new List<Tuple<TimeSpan, bool>>();

            for (TimeSpan time = new TimeSpan(0, 0, 0); time <= new TimeSpan(24, 0, 0); time += TimeSpan.FromMinutes(15))
            {
                Tuple<TimeSpan, bool> slot = new Tuple<TimeSpan, bool>(time, false);
                timeSlots.Add(slot);
            }
            var ans = new List<TimeSpan>();
            var slotIdx = (int)(startTime.TotalMinutes / 15);
            for (; slotIdx < (int)(endTime.TotalMinutes / 15); slotIdx++)
            {
                var endTimeSlot = timeSlots[slotIdx].Item1.Add(timeSlot); // End time for the time slot
                                                                          // Check if the time slot is available
                var isAvailable = true;
                foreach (var booking in bookings)
                {
                    var EndTime = booking.StartTime.Add(TimeSpan.FromHours(booking.NumberOfHours));
                    if (booking.StartTime <= timeSlots[slotIdx].Item1 && EndTime >= endTimeSlot)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                foreach (var booking in guestBooking)
                {
                    var EndTime = booking.EndTime;
                    if (booking.StartTime <= timeSlots[slotIdx].Item1 && EndTime >= endTimeSlot)
                    {
                        isAvailable = false;
                        break;
                    }
                }
                // If the time slot is available, add it to the list of available time slots
                if (isAvailable)
                {
                    ans.Add(TimeSpan.FromMinutes(slotIdx * 15));
                }
                else break;
            }
            return ans;
        }
    }
}
