using Windows.ApplicationModel.Activation;


namespace Prism.Windows
{
    public class StartArgs : IStartArgs
    {
        public StartArgs(IActivatedEventArgs args)
        {
            Arguments = args;
        }

        public StartArgs(BackgroundActivatedEventArgs args)
        {
            Arguments = args;
        }

        public object Arguments { get; private set; }
    }
}
