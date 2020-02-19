using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Serialization.Tests.Models
{
    public static class ReminderFactory
    {
        public static Reminder Create(DateTime nextDue)
        {
            return new Reminder { 
                CustomDays = Days.Monday & Days.Wednesday & Days.Friday,
                DayRange = DayRange.Custom,
                TimeRange = TimeRange.WorkDay,
                Title = "Test Reminder",
                Notes = "Exercise full range of motion",
                HowManyTimesADay = 4,
                StartDate = nextDue - TimeSpan.FromDays(7),
                EndDate = nextDue + TimeSpan.FromDays(14),
                NextDue = nextDue
            };
        }
    }
}
