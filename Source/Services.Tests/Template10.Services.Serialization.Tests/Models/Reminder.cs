using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Serialization.Tests.Models
{
    public class Reminder
    {
        public TimeRange TimeRange { get; set; }
        public DayRange DayRange { get; set; }
        public Days CustomDays { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public int HowManyTimesADay { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? NextDue { get; set; }
    }
}
