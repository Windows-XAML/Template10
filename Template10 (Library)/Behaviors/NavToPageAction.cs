using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Behaviors
{
    public class NavToPageAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            if (null == Page)
                return false;

            if (Frame == null)
                throw new NullReferenceException($"{nameof(NavToPageAction)}.{nameof(Frame)} is required.");

            var nav = NavigationService.GetForFrame(Frame);
            if (nav == null)
                throw new NullReferenceException($"Cannot find NavigationService for {Frame.ToString()}.");

            IXamlMetadataProvider metadataProvider = Application.Current as IXamlMetadataProvider;
            if (metadataProvider == null)
            {
                throw new Exception($"1 Cannot find type {Page}");
            }
            IXamlType xamlType = metadataProvider.GetXamlType(Page);
            if (xamlType == null)
            {
                throw new Exception($"2 Cannot find type {Page}");
            }
            Type type = xamlType.UnderlyingType;
            if (type == null)
            {
                throw new Exception($"3 Cannot find type {Page}");
            }
            nav.Navigate(type, Parameter, InfoOverride);
            return null;
        }

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }
        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register(nameof(Frame), typeof(Frame),
                typeof(NavToPageAction), new PropertyMetadata(null));

        public string Page
        {
            get { return (string)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register(nameof(Page), typeof(string),
                typeof(NavToPageAction), new PropertyMetadata(null));

        public string Parameter
        {
            get { return (string)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(nameof(Parameter), typeof(string),
                typeof(NavToPageAction), new PropertyMetadata(null));

        public NavigationTransitionInfo InfoOverride
        {
            get { return (NavigationTransitionInfo)GetValue(InfoOverrideProperty); }
            set { SetValue(InfoOverrideProperty, value); }
        }
        public static readonly DependencyProperty InfoOverrideProperty =
            DependencyProperty.Register(nameof(InfoOverride), typeof(NavigationTransitionInfo),
                typeof(NavToPageAction), new PropertyMetadata(null));
    }
}
