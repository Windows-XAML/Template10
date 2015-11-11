using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Template10.Controls
{
    public sealed class ModalDialog : ContentControl
    {
        public ModalDialog()
        {
            DefaultStyleKey = typeof(ModalDialog);
        }

        #region parts

        VisualStateGroup CommonStates;

        protected override void OnApplyTemplate()
        {
            CommonStates = GetTemplatedChild<VisualStateGroup>(nameof(CommonStates));
            Update();
        }

        private T GetTemplatedChild<T>(string name) where T : DependencyObject
        {
            var child = GetTemplateChild(name) as T;
            if (child == null)
                throw new NullReferenceException(name);
            return child;
        }

        #endregion  

        private void Update()
        {
            if (CommonStates == null)
                return;
            var state = (IsModal) ? "Modal" : "Normal";
            VisualStateManager.GoToState(this, state, true);
        }

        #region props

        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }
        public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register(nameof(IsModal),
            typeof(bool), typeof(ModalDialog), new PropertyMetadata(false, (d, e) => (d as ModalDialog).Update()));

        public Brush ModalBackground
        {
            get { return (Brush)GetValue(ModalBackgroundProperty); }
            set { SetValue(ModalBackgroundProperty, value); }
        }
        public static readonly DependencyProperty ModalBackgroundProperty = DependencyProperty.Register(nameof(ModalBackground),
            typeof(Brush), typeof(ModalDialog), null);

        public UIElement ModalContent
        {
            get { return (UIElement)GetValue(ModalContentProperty); }
            set { SetValue(ModalContentProperty, value); }
        }
        public static readonly DependencyProperty ModalContentProperty = DependencyProperty.Register(nameof(ModalContent),
            typeof(UIElement), typeof(ModalDialog), null);

        #endregion  
    }
}
