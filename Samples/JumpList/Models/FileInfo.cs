using Windows.Storage;

namespace Template10.Samples.JumpListSample.Models
{
    public class FileInfo
    {
        public StorageFile Ref { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
    }
}