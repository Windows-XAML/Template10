namespace Template10.Services.Settings
{
    public interface ISettingsHelper
    {
        bool EnableCompression { get; set; }

        T Read<T>(string key);
        string ReadString(string key);
        T SafeRead<T>(string key, T otherwise);
        bool TryRead<T>(string key, out T value);
        bool TryReadString(string key, out string value);

        bool TryWrite<T>(string key, T value);
        bool TryWriteString(string key, string value);
        void Write<T>(string key, T value);
        void WriteString(string key, string value);
    }
}