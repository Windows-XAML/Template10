using Windows.Foundation.Collections;

namespace Template10.Services.SettingsService
{
	public interface ISettingsService
    {
        IPropertySet Values { get; }
        T Read<T>(string key, T fallback = default(T));
        void Write<T>(string key, T value);
        void Clear(bool deep = true);
        bool Exists(string key);
        void Remove(string key);
        ISettingsService Open(string folderName, bool createFolderIfNotExists = true);
    }
}