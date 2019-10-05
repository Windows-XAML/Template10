namespace Template10.Services
{
    public static class DialogSettings
    {
        public static IDialogResourceResolver DefaultResolver { get; set; } = new DefaultResourceResolver();
    }
}
