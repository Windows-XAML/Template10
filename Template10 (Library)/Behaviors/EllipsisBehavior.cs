using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Template10.Utils;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Media;
using Template10.Controls;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-XamlBehaviors
    [TypeConstraint(typeof(CommandBar))]
    public class EllipsisBehavior : DependencyObject, IBehavior
    {
        private CommandBar commandBar => AssociatedObject as CommandBar;
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            commandBar.Loaded += CommandBar_Loaded;
            commandBar.LayoutUpdated += CommandBar_LayoutUpdated;
            commandBar.PrimaryCommands.VectorChanged += Commands_VectorChanged;
            commandBar.SecondaryCommands.VectorChanged += Commands_VectorChanged;
            Update();
        }

        public void Detach()
        {
            commandBar.Loaded -= CommandBar_Loaded;
            commandBar.LayoutUpdated += CommandBar_LayoutUpdated;
            commandBar.PrimaryCommands.VectorChanged -= Commands_VectorChanged;
            commandBar.SecondaryCommands.VectorChanged -= Commands_VectorChanged;
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e) => Update();
        private void CommandBar_LayoutUpdated(object sender, object e) => Update();
        private void Commands_VectorChanged(IObservableVector<ICommandBarElement> sender, IVectorChangedEventArgs @event) => Update();

        public enum Visibilities { Visible, Collapsed, Auto }
        public Visibilities Visibility
        {
            get { return (Visibilities)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(nameof(Visibility), typeof(Visibilities),
                typeof(EllipsisBehavior), new PropertyMetadata(Visibilities.Auto));

        private Button FindButtonInternal()
        {
            return (commandBar as PageHeader)?.GetMoreButton();
        }

        private Button FindButtonByName()
        {
            if (VisualTreeHelper.GetChildrenCount(commandBar) > 0)
            {
                var child = VisualTreeHelper.GetChild(commandBar, 0) as FrameworkElement; /* Templated root */
                return child?.FindName("MoreButton") as Button;
            }
            return null;
        }

        private Button FindButtonByTreeEnum()
        {
            var controls = XamlUtils.AllChildren<Control>(commandBar);
            return controls.OfType<Button>().FirstOrDefault(x => x.Name.Equals("MoreButton"));
        }

        private Button FindButton()
        {
            if (commandBar == null)
            {
                return null;
            }
            var r = FindButtonInternal(); // try get button from internal cached value
                                          // most optimized scenario for PageHeader control, no WinRT interop calls
            if (r != null)
            {
                return r;
            }
            r = FindButtonByName(); // try find button by name from templated root
                                    // minimized WinRT interop calls (3 calls) for MUCH better performance
            if (r != null)
            {
                return r;
            }
            return FindButtonByTreeEnum(); // fallback method - many WinRT interop calls so it is very expensive
        }

        private void Update()
        {
            var button = FindButton();
            if (button == null)
                return;
            switch (Visibility)
            {
                case Visibilities.Visible:
                    button.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    break;
                case Visibilities.Collapsed:
                    button.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    break;
                case Visibilities.Auto:
                    var count = commandBar.PrimaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Windows.UI.Xaml.Visibility.Visible));
                    count += commandBar.SecondaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(Windows.UI.Xaml.Visibility.Visible));
                    button.Visibility = (count > 0) ? Windows.UI.Xaml.Visibility.Visible : Windows.UI.Xaml.Visibility.Collapsed;
                    break;
            }

        }
    }
}
