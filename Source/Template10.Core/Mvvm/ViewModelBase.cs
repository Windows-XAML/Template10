using System.Threading.Tasks;
using Template10.Navigation;

namespace Template10.Mvvm
{
    public abstract class ViewModelBase : Prism.Mvvm.BindableBase,
        IConfirmNavigation,
        IConfirmNavigationAsync,
        IDestructible,
        INavigatedAware,
        INavigatedAwareAsync,
        INavigatingAware,
        INavigatingAwareAsync
    {
        public IPlatformNavigationService NavigationService { get; internal set; }

        public virtual bool CanNavigate(INavigationParameters parameters)
        {
            return true;
        }

        public Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            return Task.FromResult(true);
        }

        public void Destroy() { /* empty */ }

        public void OnNavigatedFrom(INavigationParameters parameters) { /* empty */ }

        public void OnNavigatedTo(INavigationParameters parameters) { /* empty */ }

        public Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public void OnNavigatingTo(INavigationParameters parameters) { /* empty */ }

        public Task OnNavigatingToAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }
    }
}
