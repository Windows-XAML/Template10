using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Services.View;
using Template10.App;
using Template10.Interfaces.Services.Navigation;

namespace Template10
{
    public abstract partial class Bootstrapper : Windows.UI.Xaml.Application
    {
        public Bootstrapper()
        {
            _current = this;
        }

        private static Bootstrapper _current;
        public static new Bootstrapper Current() => _current;

        public INavigationService NavigationService { get; set; }

        public IActivatedEventArgs ActivatedEventArgs { get; private set; }

        public SuspensionLogic LifecycleLogic { get; } = new SuspensionLogic();

        protected StateLogic State { get; } = new StateLogic();

        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            this.DebugWriteMessage($"{args.Window}");
            var viewService = new ViewService(args.Window);
        }

        protected async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            this.DebugWriteMessage($"{e}");
            if (State.Exists(BootstrapperStates.BeforeInternalActivate))
                return;
            ActivatedEventArgs = e;
            State.Add(BootstrapperStates.BeforeInternalActivate);
            await Task.CompletedTask;
            State.Add(BootstrapperStates.AfterInternalActivate);
        }

        protected async Task InternalLaunchedAsync(IActivatedEventArgs e)
        {
            this.DebugWriteMessage($"{e}");
            if (State.Exists(BootstrapperStates.BeforeInternalLaunch))
                return;
            ActivatedEventArgs = e;
            State.Add(BootstrapperStates.BeforeInternalLaunch);
            await Task.CompletedTask;
            State.Add(BootstrapperStates.AfterInternalLaunch);
        }

        public virtual async Task OnPrelaunchAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.CompletedTask;
        }

        public abstract Task OnStartAsync(IActivatedEventArgs args, StartKinds startKind);

        #region Seals

        protected sealed override async void OnActivated(IActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileActivated(FileActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnSearchActivated(SearchActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { this.DebugWriteMessage(); await InternalActivatedAsync(e); }
        protected sealed override async void OnLaunched(LaunchActivatedEventArgs e) { this.DebugWriteMessage(); await InternalLaunchedAsync(e); }

        #endregion
    }

}
