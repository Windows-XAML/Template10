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
        // void Setup();

        bool AllowBackRequested { get; set; }
        bool AllowForwardRequested { get; set; }

        event TypedEventHandler<HandledEventArgs> BackRequested;
        event TypedEventHandler<HandledEventArgs> ForwardRequested;

        event EventHandler AfterSearchGesture;
        event EventHandler AfterMenuGesture;
        event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }

    public interface IGestureService2
    {
        IBackButtonService2 BackService { get; set; }
        IKeyboardService2 KeyService { get; set; }
        event EventHandler BackRequested2;
        event EventHandler ForwardRequested2;
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

        public bool AllowBackRequested { get; set; } = true;

        public bool AllowForwardRequested { get; set; } = true;

        public GestureService(IBackButtonService backService, IKeyboardService keyService)
        {
            Two.BackService = backService as IBackButtonService2;
            Two.BackService.BackRequested += (s, e) =>
            {
                BackRequested2?.Invoke(s, e);
                if (AllowBackRequested)
                {
                    BackRequested?.Invoke(s, e);
                }
            };

            Two.BackService.ForwardRequested += (s, e) =>
            {
                ForwardRequested2?.Invoke(s, e);
                if (AllowForwardRequested)
                {
                    ForwardRequested?.Invoke(s, e);
                }
            };

            Two.KeyService = keyService as IKeyboardService2;
            Two.KeyService.AfterSearchGesture += (s, e) => AfterSearchGesture?.Invoke(s, e);
            Two.KeyService.AfterMenuGesture += (s, e) => AfterMenuGesture?.Invoke(s, e);
            Two.KeyService.AfterKeyDown += (s, e) => AfterKeyDown?.Invoke(s, e);
        }

        IBackButtonService2 IGestureService2.BackService { get; set; }
        IKeyboardService2 IGestureService2.KeyService { get; set; }
        IGestureService2 Two => this as IGestureService2;

        public event TypedEventHandler<HandledEventArgs> BackRequested;
        public event TypedEventHandler<HandledEventArgs> ForwardRequested;

        public event EventHandler BackRequested2;
        public event EventHandler ForwardRequested2;

        public event EventHandler AfterSearchGesture;
        public event EventHandler AfterMenuGesture;
        public event TypedEventHandler<KeyboardEventArgs> AfterKeyDown;
    }
}