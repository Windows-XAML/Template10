using System;
using Template10.Common;
using Template10.Utils;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Controls
{
    public sealed class ModalDialog : ContentControl
    {
        public ModalDialog()
        {
            DefaultStyleKey = typeof(ModalDialog);
            Loaded += ModalDialog_Loaded;
            Unloaded += ModalDialog_Unloaded;
        }

        private void ModalDialog_Loaded(object sender, RoutedEventArgs e)
        {
            BootStrapper.BackRequested += BootStrapper_BackRequested;
        }

        private void ModalDialog_Unloaded(object sender, RoutedEventArgs e)
        {
            BootStrapper.BackRequested -= BootStrapper_BackRequested;
        }

        private void BootStrapper_BackRequested(object sender, HandledEventArgs e)
        {
            if (!IsModal)
                return;
            if (CanBackButtonDismiss && DisableBackButtonWhenModal)
            {
                e.Handled = true;
                IsModal = false;
            }
            else if (CanBackButtonDismiss && !DisableBackButtonWhenModal)
            {
                e.Handled = IsModal;
                IsModal = false;
            }
            else if (!CanBackButtonDismiss && DisableBackButtonWhenModal)
            {
                e.Handled = true;
            }
            else if (!CanBackButtonDismiss && !DisableBackButtonWhenModal)
            {
                e.Handled = false;
            }
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

            // this switch ensures ModalTransitions plays every time.
            if (!IsModal)
            {
                var content = ModalContent;
                ModalContent = null;
                ModalContent = content;
            }
        }

        #region props

        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }
        public static readonly DependencyProperty IsModalProperty = DependencyProperty.Register(nameof(IsModal),
            typeof(bool), typeof(ModalDialog), new PropertyMetadata(false, (d, e) => (d as ModalDialog).Update()));

        public bool CanBackButtonDismiss
        {
            get { return (bool)GetValue(CanBackButtonDismissProperty); }
            set { SetValue(CanBackButtonDismissProperty, value); }
        }
        public static readonly DependencyProperty CanBackButtonDismissProperty = DependencyProperty.Register(nameof(CanBackButtonDismiss),
            typeof(bool), typeof(ModalDialog), new PropertyMetadata(false, (d, e) => (d as ModalDialog).Update()));

        public bool DisableBackButtonWhenModal
        {
            get { return (bool)GetValue(DisableBackButtonWhenModalProperty); }
            set { SetValue(DisableBackButtonWhenModalProperty, value); }
        }
        public static readonly DependencyProperty DisableBackButtonWhenModalProperty = DependencyProperty.Register(nameof(DisableBackButtonWhenModal),
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

        public TransitionCollection ModalTransitions
        {
            get { return (TransitionCollection)GetValue(ModalTransitionsProperty); }
            set { SetValue(ModalTransitionsProperty, value); }
        }
        public static readonly DependencyProperty ModalTransitionsProperty =
            DependencyProperty.Register(nameof(ModalTransitions), typeof(TransitionCollection),
                typeof(ModalDialog), new PropertyMetadata(null));

        #endregion
    }
}
