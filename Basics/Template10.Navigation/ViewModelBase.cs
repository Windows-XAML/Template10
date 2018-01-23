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
    public abstract class ViewModelBase : INavigatedAware, INavigatedAwareAsync, INavigatingAware, IConfirmNavigationAsync, IConfirmNavigation, INotifyPropertyChanged
    {
        protected void Set<T>(ref T store, T value, [CallerMemberName]string property = null)
        {
            if (Equals(store, value))
            {
                return;
            }
            store = value;
            RaisePropertyChanged(property);
        }
        protected void RaisePropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnNavigatingTo(INavigationParameters parameters) { /* empty */ }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { /* empty */ }
        public virtual Task OnNavigatedToAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual void OnNavigatedFrom(INavigationParameters parameters) {/* empty */  }
        public virtual Task OnNavigatedFromAsync(INavigationParameters parameters) => Task.CompletedTask;

        public virtual bool CanNavigate(INavigationParameters parameters) => true;
        public virtual async Task<bool> CanNavigateAsync(INavigationParameters parameters)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}
