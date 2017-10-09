using System;

namespace Template10.Services.Dialog
{
    public interface IResourceResolver
    {
        Func<ResourceTypes, string> Resolve { get; set; }
    }
}
