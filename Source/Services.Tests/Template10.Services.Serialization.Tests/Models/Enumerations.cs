using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Serialization.Tests.Models
{
    public enum TimeRange
    {
        Unknown,
        AllDay,
        WorkDay,
        Custom
    }

    public enum DayRange
    {
        Unknown,
        EveryDay,
        Weekends,
        Custom
    }

    [Flags]
    public enum Days
    {
        Unknown,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}
