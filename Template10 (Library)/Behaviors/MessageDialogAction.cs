using Microsoft.Xaml.Interactivity;
using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Behaviors
{
    public class MessageDialogAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var d = new ContentDialog { Title = Title, Content = Content, PrimaryButtonText = OkText };

            Task.Run(async () =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await d.ShowAsync());
            });

            return this;
        }

        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(string),
                typeof(MessageDialogAction), new PropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string),
                typeof(MessageDialogAction), new PropertyMetadata(string.Empty));

        public string OkText
        {
            get { return (string)GetValue(OkTextProperty); }
            set { SetValue(OkTextProperty, value); }
        }
        public static readonly DependencyProperty OkTextProperty =
            DependencyProperty.Register(nameof(OkText), typeof(string),
                typeof(MessageDialogAction), new PropertyMetadata("Ok"));

    }
}
