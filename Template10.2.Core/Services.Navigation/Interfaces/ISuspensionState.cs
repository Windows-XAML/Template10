using System;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Template10.Services.Navigation
{
    public interface ISuspensionState 
    {
        ISuspensionState Mark();
        IPropertySet Values { get; }
    }

}