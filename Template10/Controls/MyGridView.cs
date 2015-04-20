using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Template10.Controls
{
    class MyGridView : GridView
    {
        public MyGridView()
        {
            var panelXaml = "<ItemsPanelTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'><VariableSizedWrapGrid ItemHeight='{0}' ItemWidth='{1}' Orientation='Horizontal' /></ItemsPanelTemplate>";
            panelXaml = string.Format(panelXaml, this.ItemHeight, this.ItemWidth);
            this.ItemsPanel = XamlReader.Load(panelXaml) as ItemsPanelTemplate;

            var style = this.ItemContainerStyle = new Style { TargetType = typeof(GridViewItem) };
            style.Setters.Add(new Setter { Property = GridViewItem.VerticalContentAlignmentProperty, Value = VerticalAlignment.Stretch });
            style.Setters.Add(new Setter { Property = GridViewItem.HorizontalContentAlignmentProperty, Value = HorizontalAlignment.Stretch });
        }

        public double ItemHeight { get { return (double)GetValue(ItemHeightProperty); } set { SetValue(ItemHeightProperty, value); } }
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(MyGridView), new PropertyMetadata(150d));

        public double ItemWidth { get { return (double)GetValue(ItemWidthProperty); } set { SetValue(ItemWidthProperty, value); } }
        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(MyGridView), new PropertyMetadata(150d));

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            dynamic model = item;
            try
            {
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.ColumnSpanProperty, model.ColSpan);
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.RowSpanProperty, model.RowSpan);
            }
            catch
            {
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.ColumnSpanProperty, 1);
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.RowSpanProperty, 1);
            }
            finally
            {
                element.SetValue(VerticalContentAlignmentProperty, VerticalAlignment.Stretch);
                element.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch);
                base.PrepareContainerForItemOverride(element, item);
            }
        }

        // refresh the variablesizedwrapgrid layout
        public void Update()
        {
            if (!(this.ItemsPanelRoot is VariableSizedWrapGrid))
                throw new ArgumentException("ItemsPanel is not VariableSizedWrapGrid");

            foreach (var container in this.ItemsPanelRoot.Children.Cast<GridViewItem>())
            {
                dynamic data = container.Content;
                VariableSizedWrapGrid.SetRowSpan(container, data.RowSpan);
                VariableSizedWrapGrid.SetColumnSpan(container, data.ColSpan);
            }

            this.ItemsPanelRoot.InvalidateMeasure();
        }

        public interface IVariableGridItem
        {
            int ColSpan { get; set; }
            int RowSpan { get; set; }
        }
    }
}
