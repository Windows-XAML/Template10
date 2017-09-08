using System.Threading.Tasks;
using Template10.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Template10
{
    public interface IBootStraperPopupItem { }

    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Template))]
    public class BusyPopup : IBootStraperPopupItem
    {
        private Popup popup;
        private UIElement previous;

        public DataTemplate Template { get; set; }
        public bool IsShowing { get; private set; }
        public async Task ShowAsync()
        {
            if (IsShowing)
            {
                throw new System.Exception($"From {nameof(BusyPopup)}.{nameof(ShowAsync)}: {nameof(BusyPopup)}.{nameof(IsShowing)} is {IsShowing}.");
            }
            IsShowing = true;
            var presenter = new ContentPresenter
            {
                ContentTemplate = Template,
                Content = null
            };
            previous = Window.Current.Content;
            Window.Current.Content = presenter;
        }
        public void Hide()
        {
            if (!IsShowing)
            {
                throw new System.Exception($"From {nameof(BusyPopup)}.{nameof(Hide)}: {nameof(BusyPopup)}.{nameof(IsShowing)} is {IsShowing}.");
            }
            IsShowing = false;
            Window.Current.Content = previous;
        }
    }

    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Template))]
    public class SplashPopup : IBootStraperPopupItem
    {
        public DataTemplate Template { get; set; }
        public bool IsShowing { get; private set; }
        public Task ShowAsync() => null;
        public void Hide() { }
    }

    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Template))]
    public class NetworkPopup : IBootStraperPopupItem
    {
        public DataTemplate Template { get; set; }
        public bool IsShowing { get; private set; }
        public Task ShowAsync() => null;
        public void Hide() { }
    }
}
