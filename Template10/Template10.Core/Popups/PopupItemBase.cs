using System.Linq;
using Template10.Controls.Dialog;
using Template10.Core;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Popups
{
    [ContentProperty(Name = nameof(Template))]
    public abstract class PopupItemBase : IPopupItem
    {
        public PopupItemBase()
        {
            Popup = new PopupEx();
        }

        public abstract void Initialize();

        private PopupEx Popup { get; }

        public object Content { get; set; }

        public DataTemplate Template { get; set; }

        public TransitionCollection TransitionCollection { get; set; }

        public Brush BackgroundBrush { get; set; } = null;

        public bool IsShowing
        {
            get => Popup.IsShowing;
            set
            {
                if (value && !IsShowing)
                {
                    Popup.Template = Template ?? Popup.Template;
                    Popup.TransitionCollection = TransitionCollection ?? Popup.TransitionCollection;
                    Popup.BackgroundBrush = BackgroundBrush ?? Popup.BackgroundBrush;
                    Popup.Show(Content);
                }
                else if (!value && IsShowing)
                {
                    Popup.Hide();
                }
            }
        }
    }
}