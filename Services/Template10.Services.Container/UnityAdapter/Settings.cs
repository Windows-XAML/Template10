namespace Template10.Services.Container
{
    public static partial class Settings
    {
        private static IContainerAdapter _defaultAdapter;
        public static IContainerAdapter DefaultAdapter
        {
            get { return _defaultAdapter ?? (_defaultAdapter = new UnityContainerAdapter()); }
            set { _defaultAdapter = value; }
        }
    }
}
