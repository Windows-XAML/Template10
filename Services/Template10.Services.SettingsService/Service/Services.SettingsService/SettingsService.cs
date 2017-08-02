namespace Template10.Services.SettingsService
{
    public class SettingsService : SettingsServiceBase
    {
        public SettingsService(SettingsStrategies strategy, string folderName = null, bool createFolderIfNotExists = true) : base(strategy, folderName, createFolderIfNotExists)
        {
            // empty
        }
    }
}
