using System;

namespace Template10.Services
{
    public interface IDialogResourceResolver
    {
        Func<ResourceTypes, string> Resolve { get; set; }
    }
}