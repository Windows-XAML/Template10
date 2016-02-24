﻿using Microsoft.Xaml.Interactivity;
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

            var metadataProvider = Application.Current as IXamlMetadataProvider;
            if (metadataProvider == null)
                return false;

            var pagetype = metadataProvider.GetXamlType(Page as Type);
            if (pagetype == null)
                throw new NullReferenceException($"Cannot find TargetPage:{Page}");

            nav.Navigate(pagetype as Type, Parameter, InfoOverride);
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

        public object Page
        {
            get { return (object)GetValue(PageProperty); }
            set { SetValue(PageProperty, value); }
        }
        public static readonly DependencyProperty PageProperty =
            DependencyProperty.Register(nameof(Page), typeof(object),
                typeof(NavToPageAction), new PropertyMetadata(null));

        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register(nameof(Parameter), typeof(object),
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
