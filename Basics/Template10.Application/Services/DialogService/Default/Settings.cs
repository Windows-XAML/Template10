namespace Prism.Windows.Services.DialogService
{
    public static class Settings
    {
        public static IDialogResourceResolver DefaultResolver { get; set; } = new DefaultResourceResolver();
    }
}
