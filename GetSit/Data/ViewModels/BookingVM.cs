﻿using GetSit.Data.enums;
using GetSit.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace GetSit.Data.ViewModels
{
    public class BookingVM

    {
        [Required]
        public SpaceHall SelectedHall { get; set; }
        [Required]
        public Space SelectedSpace { get; set; }
        [Required]
        public List<Tuple<TimeSpan, bool>>? AvailableSlots { get; set; }
        [AllowNull]
        public Dictionary<int, int> SelectedServicesQuantities { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime DesiredDate { get; set; }
        [Required, DataType(DataType.Date)]
        public TimeSpan StartTime { get; set; }
        [Required, DataType(DataType.Date)]
        public TimeSpan EndTime { get; set; }
        [Required]
        public float TotalCost { get; set; }
        public List<Dictionary<DateTime, List<Tuple<TimeSpan, bool>>>>? SlotsForWeek { get; set; }

        public List<PaymentDetail>? paymentDetails { get; set; }
        public Customer? Customer { get; set; }
        public float ?Paid { get; set; }
        public Dictionary<int, PaymentStatus>? ServicesStatus { get; set; }
    }
}
