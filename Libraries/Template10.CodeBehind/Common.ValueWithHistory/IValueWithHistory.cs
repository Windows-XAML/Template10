using System;
using System.Collections.Generic;

namespace Template10.Portable.Common
{
    public interface IValueWithHistory<T>
    {
        T Value { get; set; }
        Dictionary<DateTime, T> History { get; }
    }
}
