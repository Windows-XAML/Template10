using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Services.Dialog
{
    public class DefaultResourceResolver : IResourceResolver
    {
        /// <summary>
        ///     Resolves the resource.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns>The string value of the resource.</returns>
        public string Resolve(ResourceTypes resource)
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
