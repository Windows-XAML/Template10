using System;

namespace Template10.Services.Dialog
{
    public class EmptyResourceResolver : IResourceResolver
    {
        public Func<ResourceTypes, string> Resolve { get; set; }
    }
}
