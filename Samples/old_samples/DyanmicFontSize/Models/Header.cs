using Windows.UI.Xaml.Documents;

namespace Template10.Samples.DynamicFontSizeSample.Models
{
    public class Header : IBlock
    {
        public string Text { get; set; }
        public Run ToRun() => new Run { Text = Text };
    }
}
