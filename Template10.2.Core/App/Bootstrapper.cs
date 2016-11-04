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
    public abstract partial class Bootstrapper : Application, ILogicHost<ISuspensionLogic>
    {
        ISuspensionLogic ILogicHost<ISuspensionLogic>.Instance { get; set; } = SuspensionLogic.Instance;

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
            this.LogInfo($"{args.Window}");
            var viewService = WindowLogic.Register(args.Window);
        }

        WindowLogic WindowLogic { get; } = new WindowLogic();

        protected async Task InternalActivatedAsync(IActivatedEventArgs e)
        {
            this.LogInfo($"{e}");
            if (State.Exists(AppStates.BeforeInternalActivate))
                return;
            ActivatedEventArgs = e;
            State.Add(AppStates.BeforeInternalActivate);
            await Task.CompletedTask;
            State.Add(AppStates.AfterInternalActivate);
        }

        protected async Task InternalLaunchedAsync(IActivatedEventArgs e)
        {
            this.LogInfo($"{e}");
            if (State.Exists(AppStates.BeforeInternalLaunch))
                return;
            ActivatedEventArgs = e;
            State.Add(AppStates.BeforeInternalLaunch);
            await Task.CompletedTask;
            State.Add(AppStates.AfterInternalLaunch);
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

        protected sealed override async void OnActivated(IActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileActivated(FileActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnSearchActivated(SearchActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnShareTargetActivated(ShareTargetActivatedEventArgs e) { this.LogInfo(); await InternalActivatedAsync(e); }
        protected sealed override async void OnLaunched(LaunchActivatedEventArgs e) { this.LogInfo(); await InternalLaunchedAsync(e); }

        #endregion
    }

}
