using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Common
{
    public delegate void TypedEventHandler<T>(object sender, T e);

    public class EventArgs<T> : EventArgs { public T Value { get; set; } }

    public class CancelEventArgs<T> : System.ComponentModel.CancelEventArgs { public T Value { get; set; } }
}
