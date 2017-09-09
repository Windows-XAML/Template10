using System;
using System.Collections.Generic;

namespace Template10.Portable.Common
{
    public interface IValueWithHistory<T>
    {
        T Value { get; set; }
        Dictionary<string, T> History { get; }
    }
}
