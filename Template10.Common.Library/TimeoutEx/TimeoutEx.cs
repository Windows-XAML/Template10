using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.Common
{
    public class TimeoutEx : ITimeoutEx
    {
        public static void InvokeAfter(Action action, TimeSpan wait)
        {
            var t = new TimeoutEx(wait);
            t.Start(action);
        }

        private DispatcherTimer _timer;
        private Action _callback;

        public TimeoutEx(TimeSpan wait)
        {
            _timer = new DispatcherTimer { Interval = wait };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            _timer.Tick -= Timer_Tick;
            Parallel.Invoke(
                () => Tick?.Invoke(this, EventArgs.Empty),
                () => _callback?.Invoke());
        }

        public void Start(Action callback)
        {
            _callback = callback;
            Stop();
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        public event EventHandler Tick;
    }
}
