using System;
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
    public abstract class PopupItemBase<T> : IPopupItem, IPopupItem<T>, IPopupItem2 where T : class, IPopupData
    {
        public PopupItemBase()
        {
            Two.Popup = new PopupEx();

            var action = new Action(() => { IsShowing = false; });
            Content = Activator.CreateInstance(typeof(T), new object[] { action, Window.Current.Dispatcher }) as T;
        }

        IPopupItem2 Two => this as IPopupItem2;

        public abstract void Initialize();

        PopupEx IPopupItem2.Popup { get; set; }

        public T Content { get; set; }

        public DataTemplate Template { get; set; }

        public TransitionCollection TransitionCollection { get; set; }

        public Brush BackgroundBrush { get; set; } = null;

        public bool IsShowing
        {
            get => Two.Popup.IsShowing;
            set
            {
                if (value && !IsShowing)
                {
                    Two.Popup.Template = Template ?? Two.Popup.Template;
                    Two.Popup.TransitionCollection = TransitionCollection ?? Two.Popup.TransitionCollection;
                    Two.Popup.BackgroundBrush = BackgroundBrush ?? Two.Popup.BackgroundBrush;
                    Two.Popup.Show(Content);
                }
                else if (!value && IsShowing)
                {
                    Two.Popup.Hide();
                }
                Content.IsActive = value;
            }
        }
    }
}