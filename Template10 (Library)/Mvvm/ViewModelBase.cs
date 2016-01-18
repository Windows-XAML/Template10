using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Template10.Mvvm
{
    // DOCS: https://github.com/Windows-XAML/Template10/wiki/Docs-%7C-MVVM
    public abstract class ViewModelBase : BindableBase, INavigable
    {
        private volatile bool isNavigatedTo;
        private volatile bool isNavigatedFrom;

        [JsonIgnore]
        public bool IsNavigatedTo => isNavigatedTo;
        [JsonIgnore]
        public bool IsNavigatedFrom => isNavigatedFrom;

        #region Implicit Implementation of INavigable

        public virtual void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state) { /* nothing by default */ }
        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending) => Task.CompletedTask;
        public virtual void OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args) { /* nothing by default */ }

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }
        [JsonIgnore]
        public IDispatcherWrapper Dispatcher { get; set; }
        [JsonIgnore]
        public IStateItems SessionState { get; set; }

        #endregion

        #region Overrides of BindableBase

        public override void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (!this.isNavigatedFrom)
                base.RaisePropertyChanged(propertyName);
        }

        public override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (!this.isNavigatedFrom)
                base.RaisePropertyChanged(propertyExpression);
        }

        #endregion

        #region Explicit Implementation of INavigable

        void INavigable.OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            isNavigatedTo = true;
            isNavigatedFrom = false;
            OnNavigatedTo(parameter, mode, state);
        }

        void INavigable.OnNavigatingFrom(Services.NavigationService.NavigatingEventArgs args)
        {
            OnNavigatingFrom(args);
            if (!args.Cancel)
            {
                isNavigatedTo = false;
                isNavigatedFrom = true;
            }
        }

        #endregion
    }
}