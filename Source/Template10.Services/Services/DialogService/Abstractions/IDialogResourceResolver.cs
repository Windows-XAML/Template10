using System;

namespace Template10.Services.Dialog
{
    public interface IDialogResourceResolver
    {
        Func<ResourceTypes, string> Resolve { get; set; }
    }
}