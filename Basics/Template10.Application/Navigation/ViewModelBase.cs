using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Navigation
{
    public abstract class ViewModelBase : Prism.Mvvm.BindableBase, INavigatedAware, INavigatedAwareAsync, INavigatingAware, IConfirmNavigationAsync, IConfirmNavigation, INotifyPropertyChanged, IDestructible
    {
        // INavigatingAware

        public virtual void OnNavigatingTo(INavigationParameters parameters) { /* empty */ }

        // INavigatedAware

        public virtual void OnNavigatedTo(INavigationParameters parameters) { /* empty */ }
        public virtual void OnNavigatedFrom(INavigationParameters parameters) {/* empty */  }

        // INavigatedAwareAsync

        public virtual Task OnNavigatedToAsync(INavigationParameters parameters) => Task.CompletedTask;

        // IConfirmNavigation

        public virtual bool CanNavigate(INavigationParameters parameters) => true;

        // IConfirmNavigationAsync

        public virtual async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            await Task.CompletedTask;
            return true;
        }

        // IDestructible

        public virtual void Destroy() { /* empty */ }
    }
}
