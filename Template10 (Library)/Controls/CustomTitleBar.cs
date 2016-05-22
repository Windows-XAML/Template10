using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Controls
{
    public class CustomTitleBar : Control
    {
        private ApplicationViewTitleBar _titleBar;
        internal CustomTitleBar()
        {
            _titleBar = ApplicationView.GetForCurrentView().TitleBar;
        }

        public bool Extended
        {
            get { return CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar; }
            set { CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = value; }
        }
        public static readonly DependencyProperty ExtendedProperty =
            DependencyProperty.Register(nameof(Extended), typeof(bool), typeof(CustomTitleBar), new PropertyMetadata(false, (d, e) =>
            { (d as CustomTitleBar).Extended = (bool)e.NewValue; }));

        public Color BackgroundColor
        {
            get { return _titleBar.BackgroundColor ?? Colors.Transparent; }
            set { _titleBar.BackgroundColor = value; }
        }
        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register(nameof(BackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).BackgroundColor = (Color)e.NewValue; }));

        public Color ForegroundColor
        {
            get { return _titleBar.ForegroundColor ?? Colors.Transparent; }
            set { _titleBar.ForegroundColor = value; }
        }
        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register(nameof(ForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ForegroundColor = (Color)e.NewValue; }));

        public Color ButtonBackgroundColor
        {
            get { return _titleBar.ButtonBackgroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonBackgroundColor = value; }
        }
        public static readonly DependencyProperty ButtonBackgroundColorProperty =
            DependencyProperty.Register(nameof(ButtonBackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonBackgroundColor = (Color)e.NewValue; }));

        public Color ButtonForegroundColor
        {
            get { return _titleBar.ButtonForegroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonForegroundColor = value; }
        }
        public static readonly DependencyProperty ButtonForegroundColorProperty =
            DependencyProperty.Register(nameof(ButtonForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonForegroundColor = (Color)e.NewValue; }));

        public Color ButtonHoverBackgroundColor
        {
            get { return _titleBar.ButtonHoverBackgroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonHoverBackgroundColor = value; }
        }
        public static readonly DependencyProperty ButtonHoverBackgroundColorProperty =
            DependencyProperty.Register(nameof(ButtonHoverBackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonHoverBackgroundColor = (Color)e.NewValue; }));

        public Color ButtonHoverForegroundColor
        {
            get { return _titleBar.ButtonHoverForegroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonHoverForegroundColor = value; }
        }
        public static readonly DependencyProperty ButtonHoverForegroundColorProperty =
            DependencyProperty.Register(nameof(ButtonHoverForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonHoverForegroundColor = (Color)e.NewValue; }));

        public Color ButtonPressedBackgroundColor
        {
            get { return _titleBar.ButtonPressedBackgroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonPressedBackgroundColor = value; }
        }
        public static readonly DependencyProperty ButtonPressedBackgroundColorProperty =
            DependencyProperty.Register(nameof(ButtonPressedBackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonPressedBackgroundColor = (Color)e.NewValue; }));

        public Color ButtonPressedForegroundColor
        {
            get { return _titleBar.ButtonPressedForegroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonPressedForegroundColor = value; }
        }
        public static readonly DependencyProperty ButtonPressedForegroundColorProperty =
            DependencyProperty.Register(nameof(ButtonPressedForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonPressedForegroundColor = (Color)e.NewValue; }));

        public Color ButtonInactiveBackgroundColor
        {
            get { return _titleBar.ButtonInactiveBackgroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonInactiveBackgroundColor = value; }
        }
        public static readonly DependencyProperty ButtonInactiveBackgroundColorProperty =
            DependencyProperty.Register(nameof(ButtonInactiveBackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonInactiveBackgroundColor = (Color)e.NewValue; }));

        public Color ButtonInactiveForegroundColor
        {
            get { return _titleBar.ButtonInactiveForegroundColor ?? Colors.Transparent; }
            set { _titleBar.ButtonInactiveForegroundColor = value; }
        }
        public static readonly DependencyProperty ButtonInactiveForegroundColorProperty =
            DependencyProperty.Register(nameof(ButtonInactiveForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).ButtonInactiveForegroundColor = (Color)e.NewValue; }));

        public Color InactiveBackgroundColor
        {
            get { return _titleBar.InactiveBackgroundColor ?? Colors.Transparent; }
            set { _titleBar.InactiveBackgroundColor = value; }
        }
        public static readonly DependencyProperty InactiveBackgroundColorProperty =
            DependencyProperty.Register(nameof(InactiveBackgroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).InactiveBackgroundColor = (Color)e.NewValue; }));

        public Color InactiveForegroundColor
        {
            get { return _titleBar.InactiveForegroundColor ?? Colors.Transparent; }
            set { _titleBar.InactiveForegroundColor = value; }
        }
        public static readonly DependencyProperty InactiveForegroundColorProperty =
            DependencyProperty.Register(nameof(InactiveForegroundColor), typeof(Color), typeof(CustomTitleBar), new PropertyMetadata(null, (d, e) =>
            { (d as CustomTitleBar).InactiveForegroundColor = (Color)e.NewValue; }));

        public static void Setup()
        {
            // this "unused" bit is very important because of a quirk in ResourceThemes
            try
            {
                if (Application.Current.Resources.ContainsKey("ExtendedSplashBackground"))
                {
                    var unused = Application.Current.Resources["ExtendedSplashBackground"];
                }
            }
            catch { /* this is okay */ }

            // this wonky style of loop is important due to a platform bug
            int count = Application.Current.Resources.Count;
            foreach (var resource in Application.Current.Resources)
            {
                var k = resource.Key;
                if (k == typeof(Controls.CustomTitleBar))
                {
                    var s = resource.Value as Style;
                    var t = new Controls.CustomTitleBar();
                    t.Style = s;
                }
                count--;
                if (count == 0) break;
            }
        }
    }
}
