using System;

namespace Template10.Services.Messenger
{
    public static class Settings
    {
        private static IMessengerAdapter _defaultAdapter;
        public static IMessengerAdapter DefaultAdapter
        {
            get { return _defaultAdapter ?? (_defaultAdapter = new MvvmLightMessengerAdapter()); }
            set { _defaultAdapter = value; }
        }
    }
}
