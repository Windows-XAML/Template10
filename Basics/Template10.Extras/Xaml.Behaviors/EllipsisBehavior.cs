using Microsoft.Xaml.Interactivity;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using xaml = Windows.UI.Xaml;

namespace Template10.Behaviors
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Behaviors-and-Actions
    [TypeConstraint(typeof(CommandBar))]
    public class EllipsisBehavior : DependencyObject, IBehavior
    {
        private CommandBar _commandBar => AssociatedObject as CommandBar;

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            _commandBar.Loaded += CommandBar_Loaded;
            _commandBar.PrimaryCommands.VectorChanged += Commands_VectorChanged;
            _commandBar.SecondaryCommands.VectorChanged += Commands_VectorChanged;
            Update();
        }

        public void Detach()
        {
            _commandBar.Loaded -= CommandBar_Loaded;
            _commandBar.PrimaryCommands.VectorChanged -= Commands_VectorChanged;
            _commandBar.SecondaryCommands.VectorChanged -= Commands_VectorChanged;
        }

        private void CommandBar_Loaded(object sender, RoutedEventArgs e) => Update();

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

        private Button FindButtonByName()
        {
            if (VisualTreeHelper.GetChildrenCount(_commandBar) > 0)
            {
                var child = VisualTreeHelper.GetChild(_commandBar, 0) as FrameworkElement; /* Templated root */
                return child?.FindName("MoreButton") as Button;
            }
            return null;
        }

        private Button FindButtonByTreeEnum()
        {
            var controls = AllChildren(_commandBar).OfType<Control>();
            return controls.OfType<Button>().FirstOrDefault(x => x.Name.Equals("MoreButton"));
        }

        private List<DependencyObject> AllChildren(DependencyObject parent)
        {
            var list = new List<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                list.AddRange(AllChildren(AddAndReturn(list, child)));
            }
            return list;
        }

        private T AddAndReturn<T>(IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        private Button FindButton()
        {
            if (_commandBar == null)
            {
                return null;
            }
            var r = FindButtonByName(); // try find button by name from templated root
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
            {
                return;
            }

            switch (Visibility)
            {
                case Visibilities.Visible:
                    button.Visibility = xaml.Visibility.Visible;
                    break;
                case Visibilities.Collapsed:
                    button.Visibility = xaml.Visibility.Collapsed;
                    break;
                case Visibilities.Auto:
                    var count = _commandBar.PrimaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(xaml.Visibility.Visible));
                    count += _commandBar.SecondaryCommands.OfType<Control>().Count(x => x.Visibility.Equals(xaml.Visibility.Visible));
                    button.Visibility = (count > 0) ? xaml.Visibility.Visible : xaml.Visibility.Collapsed;
                    break;
            }
        }
    }
}
