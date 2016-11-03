using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Template10.Services.Lifetime;
using Template10.App;
using Template10.Services.Navigation;
using Template10.BCL;
using System;

namespace Template10.App
{
    public abstract partial class Bootstrapper : Application, ILogicHost<ISuspensionService>
    {
        ISuspensionService ILogicHost<ISuspensionService>.Instance { get; set; } = SuspensionService.Instance;

        public Bootstrapper()
        {
            _current = this;
        }

        private static Bootstrapper _current;
        public static new Bootstrapper Current() => _current;

        public INavigationService NavigationService { get; set; }

        public IActivatedEventArgs ActivatedEventArgs { get; private set; }

        protected StateLogic State { get; } = new StateLogic();

        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            this.DebugWriteInfo($"{args.Window}");
            var viewService = WindowLogic.Register(args.Window);
        }

        WindowLogic WindowLogic { get; } = new WindowLogic();

        protected async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            this.DebugWriteInfo($"{e}");
            if (State.Exists(BootstrapperStates.BeforeInternalActivate))
                return;
            ActivatedEventArgs = e;
            State.Add(BootstrapperStates.BeforeInternalActivate);
            await Task.CompletedTask;
            State.Add(BootstrapperStates.AfterInternalActivate);
        }

        protected async Task InternalLaunchedAsync(IActivatedEventArgs e)
        {
            this.DebugWriteInfo($"{e}");
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

        protected sealed override async void OnActivated(IActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileActivated(FileActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnSearchActivated(SearchActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { this.DebugWriteInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnLaunched(LaunchActivatedEventArgs e) { this.DebugWriteInfo(); await InternalLaunchedAsync(e); }

        #endregion
    }

}
