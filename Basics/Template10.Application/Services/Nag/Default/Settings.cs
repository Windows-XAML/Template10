using Prism.Windows.Services.DialogService;

namespace Prism.Windows.Services.Nag
{
    public static class Settings
    {
        public static IDialogResourceResolver CustomResolver { get; set; } = null;
    }
}
