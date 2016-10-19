using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Template10.Helpers;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Template10.App
{
    public abstract partial class Bootstrapper : Windows.UI.Xaml.Application
    {
        public Bootstrapper()
        {
            _current = this;
        }

        private static Bootstrapper _current;
        public static new Bootstrapper Current() => _current;

        protected sealed override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            var viewService = new View.ViewService(args.Window);
        }

        public virtual async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            await Task.Delay(0);
        }

        public abstract Task OnStartAsync(IActivatedEventArgs args, StartKind startKind);

        internal LoadStates State { get; set; }

        internal Dictionary<DateTime, LoadStates> StateHistory = new Dictionary<DateTime, LoadStates>();

        internal LifecycleLogic LifecycleLogic = new LifecycleLogic();
    }

}
