using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SampleData.Data.StarTrek
{
    public partial class Image
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        public ImageSource ImageSource => new BitmapImage(new System.Uri($"ms-appx:///SampleData/StarTrek/Images/{Path}", System.UriKind.Absolute));
    }
}