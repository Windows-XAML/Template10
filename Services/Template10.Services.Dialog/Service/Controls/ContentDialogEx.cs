using System;
using System.Threading.Tasks;
using Template10.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{
    public enum DismissModes
    {
        // allow System.BackRequested
        Default,
        // require Dialog.Hide()
        Programmatic
    }

    public enum ShowingStates
    {
        Queued,
        Showing,
        Closed,
        None
    }

    public sealed class ContentDialogEx : ContentDialog
    {
        private bool _programmaticCloseRequested;

        public ContentDialogEx()
        {
            DefaultStyleKey = typeof(ContentDialogEx);

            // for testing - secondary button click equates to programmatic hide request
            SecondaryButtonClick += (sender, args)
                => _programmaticCloseRequested = true;

            Opened += (s, e)
                => Showing = ShowingStates.Showing;

            Closed += (s, e)
                => Showing = ShowingStates.Closed;

            Closing += (s, e)
                => e.Cancel = (DismissMode == DismissModes.Programmatic && !_programmaticCloseRequested);
        }

        public DismissModes DismissMode
        {
            get => (DismissModes)GetValue(DismissModeProperty);
            set => SetValue(DismissModeProperty, value);
        }
        public static readonly DependencyProperty DismissModeProperty
            = DependencyProperty.Register(nameof(DismissMode), typeof(DismissModes),
                typeof(ContentDialogEx), new PropertyMetadata(DismissModes.Default));

        public ShowingStates Showing
        {
            get { return (ShowingStates)GetValue(ShowingProperty); }
            internal set { SetValue(ShowingProperty, value); }
        }
        public static readonly DependencyProperty ShowingProperty =
            DependencyProperty.Register(nameof(Showing), typeof(ShowingStates),
                typeof(ContentDialogEx), new PropertyMetadata(ShowingStates.None));

        public new void Hide()
        {
            _programmaticCloseRequested = true;
            base.Hide();
        }

        internal new async Task<ContentDialogResult> ShowAsync() => await base.ShowAsync();
    }
}
