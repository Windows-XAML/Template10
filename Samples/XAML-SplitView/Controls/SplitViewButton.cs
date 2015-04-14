using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Controls
{
    class SplitViewButton : RadioButton
    {
        public SplitViewButton()
        {
            this.GroupName = "SplitView";
            this.Checked += (s, e) => this.IsChecked = this.AllowIsChecked;
        }

        public UIElement Glyph { get { return (UIElement)GetValue(GlyphProperty); } set { SetValue(GlyphProperty, value); } }
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(UIElement), typeof(SplitViewButton), new PropertyMetadata(""));

       public bool AllowIsChecked { get { return (bool)GetValue(AllowIsCheckedProperty); } set { SetValue(AllowIsCheckedProperty, value); } }
        public static readonly DependencyProperty AllowIsCheckedProperty = DependencyProperty.Register("AllowIsChecked", typeof(bool), typeof(SplitViewButton), new PropertyMetadata(true));
    }
}
