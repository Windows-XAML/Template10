namespace Template10.Services.Dialog
{
    public static class Settings
    {
        public static IDialogResourceResolver DefaultResolver { get; set; } = new DefaultResourceResolver();
    }
}
