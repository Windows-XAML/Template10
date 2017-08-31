using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System;
using System.Linq.Expressions;
using Template10.Navigation;
using Template10.Core;
using Template10.Extensions;
using Template10.Common;
using Template10.Services.Container;
using Template10.Services.Resources;
using Template10.Services.Logging;

namespace Template10.Mvvm
{
    public abstract class ViewModelBase : BindableBase, INavigatedToAwareAsync, INavigatedFromAwareAsync, IConfirmNavigationAsync, ITemplate10ViewModel
    {
        /// <remarks>
        /// It is not necessary to call base.OnNavigatedToAsync in the concrete override
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async virtual Task OnNavigatedToAsync(INavigatedToParameters parameter) => await Task.CompletedTask;

        public async virtual Task OnNavigatedFromAsync(INavigatedFromParameters parameters) => await Task.CompletedTask;

        public async virtual Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters) => true;

        // add helper properties / ITemplate10ViewModel

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public IWindowEx Window => NavigationService.GetWindow();

        [JsonIgnore]
        public IDispatcherEx Dispatcher => Window.Dispatcher;

        [JsonIgnore]
        public ISessionState SessionState => Central.SessionState;

        // remove bindable overrides

        public sealed override void RaisePropertyChanged([CallerMemberName] string propertyName = null) => base.RaisePropertyChanged(propertyName);

        public sealed override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) => base.RaisePropertyChanged(propertyExpression);

        public new bool Set<T>(Expression<Func<T>> propertyExpression, ref T storage, T value) => base.Set(propertyExpression, ref storage, value);

        protected new bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) => base.Set(ref storage, value, propertyName);

        public new void Set(Action action, [CallerMemberName] string propertyName = null) => base.Set(action, propertyName);

        // remove object overrides

        public sealed override bool Equals(object obj) => base.Equals(obj);

        public sealed override int GetHashCode() => base.GetHashCode();

        public sealed override string ToString() => base.ToString();
    }
}