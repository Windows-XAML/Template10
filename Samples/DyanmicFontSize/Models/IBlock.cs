namespace Template10.Samples.DynamicFontSizeSample.Models
{
    public interface IBlock
    {
        string Text { get; set; }
        Windows.UI.Xaml.Documents.Run ToRun();
    }
}
