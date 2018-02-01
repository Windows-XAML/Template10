using System;

namespace Prism.Windows.Services.DialogService
{
    public interface IDialogResourceResolver
    {
        Func<ResourceTypes, string> Resolve { get; set; }
    }
}