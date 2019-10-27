using System;

namespace Template10.Services
{
    public enum BlockerPeriod { Always, Once }

    public class GestureBlocker
    {
        private readonly BlockerPeriod _period;
        public GestureBlocker(Gesture gesture, BlockerPeriod period)
        {
            Gesture = gesture;
            _period = period;
        }
        public Gesture Gesture { get; }
        public event EventHandler OnEvent;
        public Action EventCallback { set; get; }
        public Action Remove { internal set; get; }
        internal void RaiseEvent()
        {
            OnEvent?.Invoke(this, EventArgs.Empty);
            EventCallback?.Invoke();
            if (_period == BlockerPeriod.Once)
            {
                Remove?.Invoke();
            }
        }
    }
}