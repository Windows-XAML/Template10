using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Utils;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace DyanmicFontSize.AttachedProperties
{
    public class RichTextBlockEx : DependencyObject
    {
        public static double GetHeaderFontSize(RichTextBlock obj) { return (double)obj.GetValue(HeaderFontSizeProperty); }
        public static void SetHeaderFontSize(RichTextBlock obj, double value) { obj.SetValue(HeaderFontSizeProperty, value); }
        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.RegisterAttached("HeaderFontSize", typeof(double), typeof(RichTextBlockEx), new PropertyMetadata(16d, (d, e) => Refresh(d as RichTextBlock)));

        public static double GetHeaderIndent(RichTextBlock obj) { return (double)obj.GetValue(HeaderIndentProperty); }
        public static void SetHeaderIndent(RichTextBlock obj, double value) { obj.SetValue(HeaderIndentProperty, value); }
        public static readonly DependencyProperty HeaderIndentProperty =
            DependencyProperty.RegisterAttached("HeaderIndent", typeof(double), typeof(RichTextBlockEx), new PropertyMetadata(0d, (d, e) => Refresh(d as RichTextBlock)));

        public static double GetBodyFontSize(RichTextBlock obj) { return (double)obj.GetValue(BodyFontSizeProperty); }
        public static void SetBodyFontSize(RichTextBlock obj, double value) { obj.SetValue(BodyFontSizeProperty, value); }
        public static readonly DependencyProperty BodyFontSizeProperty =
            DependencyProperty.RegisterAttached("BodyFontSize", typeof(double), typeof(RichTextBlockEx), new PropertyMetadata(16d, (d, e) => Refresh(d as RichTextBlock)));

        public static double GetBodyIndent(RichTextBlock obj) { return (double)obj.GetValue(BodyIndentProperty); }
        public static void SetBodyIndent(RichTextBlock obj, double value) { obj.SetValue(BodyIndentProperty, value); }
        public static readonly DependencyProperty BodyIndentProperty =
            DependencyProperty.RegisterAttached("BodyIndent", typeof(double), typeof(RichTextBlockEx), new PropertyMetadata(16d, (d, e) => Refresh(d as RichTextBlock)));

        public static IEnumerable<object> GetItemsSource(RichTextBlock obj) { return (IEnumerable<object>)obj.GetValue(ItemsSourceProperty); }
        public static void SetItemsSource(RichTextBlock obj, IEnumerable<object> value) { obj.SetValue(ItemsSourceProperty, value); }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.RegisterAttached("ItemsSource", typeof(IEnumerable<object>), typeof(RichTextBlockEx), new PropertyMetadata(null, (d, e) => Fill(d as RichTextBlock)));

        private static void Refresh(RichTextBlock textblock)
        {
            var itemsSource = GetItemsSource(textblock) as IEnumerable<Models.IBlock>;
            if (itemsSource == null)
                return;
            var items = itemsSource.Select((x, i) => new { Block = x, Index = i });
            foreach (var block in (textblock.Blocks).Select((x, i) => new { Paragraph = x as Paragraph, Index = i }))
            {
                var item = items.First(x => x.Index == block.Index).Block;
                var size = FontSize(item, textblock);
                if (!Equals(block.Paragraph.FontSize, size))
                    block.Paragraph.FontSize = size;
                var indent = Indent(item, textblock);
                if (!Equals(block.Paragraph.TextIndent, indent))
                    block.Paragraph.TextIndent = indent;
            }
        }

        private static void Fill(RichTextBlock textblock)
        {
            var itemsSource = GetItemsSource(textblock) as IEnumerable<Models.IBlock>;
            if (itemsSource == null)
                return;
            textblock.Blocks.Clear();
            foreach (var item in itemsSource)
            {
                var paragraph = new Paragraph
                {
                    Margin = new Thickness(0, 0, 0, 16d),
                    FontSize = FontSize(item, textblock),
                    TextIndent = Indent(item, textblock),
                };
                if (item is Models.Header)
                    paragraph.Foreground = Colors.Black.ToSolidColorBrush();
                paragraph.Inlines.Add(item.ToRun());
                textblock.Blocks.Add(paragraph);
            }
        }

        private static double FontSize(Models.IBlock block, RichTextBlock textblock)
        {
            if (block is Models.Header)
                return GetHeaderFontSize(textblock);
            else if (block is Models.Body)
                return GetBodyFontSize(textblock);
            else
                return 16;
        }

        private static double Indent(Models.IBlock block, RichTextBlock textblock)
        {
            if (block is Models.Header)
                return GetHeaderIndent(textblock);
            else if (block is Models.Body)
                return GetBodyIndent(textblock);
            else
                return 16;
        }

    }
}
