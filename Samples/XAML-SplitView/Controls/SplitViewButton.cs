using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    class SplitViewButton : RadioButton
    {
        public SplitViewButton()
        {
            this.GroupName = "SplitView";
            this.Checked += (s, e) => this.IsChecked = this.AllowIsChecked;
            this.DefaultStyleKey = typeof(RadioButton);
        }

        public string Glyph { get { return (string)GetValue(GlyphProperty); } set { SetValue(GlyphProperty, value); } }
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(SplitViewButton), new PropertyMetadata(""));

        public FontFamily GlyphFontFamily { get { return (FontFamily)GetValue(GlyphFontFamilyProperty); } set { SetValue(GlyphFontFamilyProperty, value); } }
        public static readonly DependencyProperty GlyphFontFamilyProperty = DependencyProperty.Register("GlyphFontFamily", typeof(FontFamily), typeof(SplitViewButton), new PropertyMetadata(new FontFamily("Segoe MDL2 Assets")));

        public double GlyphFontSize { get { return (double)GetValue(GlyphFontSizeProperty); } set { SetValue(GlyphFontSizeProperty, value); } }
        public static readonly DependencyProperty GlyphFontSizeProperty = DependencyProperty.Register("GlyphFontSize", typeof(double), typeof(SplitViewButton), new PropertyMetadata(16d));

        public bool AllowIsChecked { get { return (bool)GetValue(AllowIsCheckedProperty); } set { SetValue(AllowIsCheckedProperty, value); } }
        public static readonly DependencyProperty AllowIsCheckedProperty = DependencyProperty.Register("AllowIsChecked", typeof(bool), typeof(SplitViewButton), new PropertyMetadata(true));
    }
}
