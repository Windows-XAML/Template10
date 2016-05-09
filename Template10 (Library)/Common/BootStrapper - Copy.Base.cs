using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Template10.Controls;
using Template10.Services.NavigationService;
using Template10.Utils;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Common
{
    public abstract partial class BootStrapper : Application, INotifyPropertyChanged
    {
        protected override sealed async void OnActivated(IActivatedEventArgs e) { DebugWrite(); await InternalActivatedAsync(e); }

        protected override sealed async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed async void OnFileActivated(FileActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed async void OnSearchActivated(SearchActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed async void OnShareTargetActivated(ShareTargetActivatedEventArgs args) { DebugWrite(); await InternalActivatedAsync(args); }

        protected override sealed void OnLaunched(LaunchActivatedEventArgs e) { DebugWrite(); InternalLaunchAsync(e); }
    }
}
