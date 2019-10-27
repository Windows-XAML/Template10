using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Template10.SampleData.StarTrek
{
    public partial class Image
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        public ImageSource ImageSource => new BitmapImage(new System.Uri(Path, System.UriKind.Absolute));
    }
}