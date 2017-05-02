using Windows.ApplicationModel.Activation;

namespace Template10.Common
{
	public interface ISplashLogic
    {
        bool Splashing { get; }
        void Hide();
        void Show(SplashScreen splashScreen, IBootStrapper bootstrapper);
    }
}