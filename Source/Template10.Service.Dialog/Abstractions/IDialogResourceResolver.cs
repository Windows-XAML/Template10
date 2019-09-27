using System;

namespace Template10.Service.Dialog
{
    public interface IDialogResourceResolver
    {
        Func<ResourceTypes, string> Resolve { get; set; }
    }
}