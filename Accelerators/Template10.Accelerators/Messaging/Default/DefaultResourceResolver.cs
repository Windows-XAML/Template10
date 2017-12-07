using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Dialog
{

    public class DefaultResourceResolver : EmptyResourceResolver
    {
        public DefaultResourceResolver() => Resolve = ResolveImplementation;

        public string ResolveImplementation(ResourceTypes resource)
        {
            switch (resource)
            {
                case ResourceTypes.Ok: return "Ok";
                case ResourceTypes.Yes: return "Yes";
                case ResourceTypes.No: return "No";
                case ResourceTypes.Cancel: return "Cancel";
                default: throw new NotSupportedException($"Resource:{resource}");
            }
        }
    }
}
