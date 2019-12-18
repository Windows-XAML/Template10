using System;
using System.Threading.Tasks;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class MessageAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter) => ExecuteAsync(sender, parameter);

        private bool _busy = false;
        public async Task ExecuteAsync(object sender, object parameter)
        {
            if (_busy)
            {
                return;
            }
            _busy = true;
            try
            {
                var dialog = new ContentDialog
                {
                    Title = Title,
                    Content = Content,
                    PrimaryButtonText = ButtonText
                };
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await dialog.ShowAsync());
            }
            finally
            {
                _busy = false;
            }
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string),
                typeof(MessageAction), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string),
                typeof(MessageAction), new PropertyMetadata(string.Empty));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string),
                typeof(MessageAction), new PropertyMetadata("Close"));
    }
}
