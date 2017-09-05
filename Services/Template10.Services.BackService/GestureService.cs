using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Services.Gesture
{
    public interface IGestureService
    {
        void Setup();

        event TypedEventHandler<HandledEventArgs> BackRequested;
        event TypedEventHandler<HandledEventArgs> ForwardRequested;

        event EventHandler AfterSearchGesture;
        event EventHandler AfterMenuGesture;
        event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }

    public interface IGestureService2
    {
        IBackButtonService BackService { get; set; }
        IKeyboardService KeyService { get; set; }
    }

    public class GestureService : IGestureService, IGestureService2
    {
        internal static IGestureService GetDefault()
        {
            var container = Services.Container.ContainerService.Default;
            return container.Resolve<IGestureService>();
        }

        internal static IGestureService2 GetDefault2()
        {
            var container = Services.Container.ContainerService.Default;
            return container.Resolve<IGestureService2>();
        }

        public GestureService(IBackButtonService backService, IKeyboardService keyService)
        {
            Two.BackService = backService;
            backService.BackRequested += (s, e) => BackRequested?.Invoke(s, e);
            backService.ForwardRequested += (s, e) => ForwardRequested?.Invoke(s, e);

            Two.KeyService = keyService;
            keyService.AfterSearchGesture += (s, e) => AfterSearchGesture?.Invoke(s, e);
            keyService.AfterMenuGesture += (s, e) => AfterMenuGesture?.Invoke(s, e);
            keyService.AfterKeyDown += (s, e) => AfterKeyDown?.Invoke(s, e);
        }

        IBackButtonService IGestureService2.BackService { get; set; }
        IKeyboardService IGestureService2.KeyService { get; set; }
        IGestureService2 Two => this as IGestureService2;

        public void Setup()
        {
            Two.BackService.Setup();
            Two.KeyService.Setup();
        }

        public event TypedEventHandler<HandledEventArgs> BackRequested;
        public event TypedEventHandler<HandledEventArgs> ForwardRequested;

        public event EventHandler AfterSearchGesture;
        public event EventHandler AfterMenuGesture;
        public event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }
}