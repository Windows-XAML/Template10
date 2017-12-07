using Template10.Services.StateService;

namespace Template10.Portable.Navigation
{

    public interface INavigationParameters
    {
        INavigationInfo FromNavigationInfo { get; }
        INavigationInfo ToNavigationInfo { get; }
        IStateContainer SessionState { get; }
    }
}