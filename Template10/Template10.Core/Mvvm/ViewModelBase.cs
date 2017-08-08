using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Navigation;
using Template10.Mvvm;
using Template10.Extensions;
using Template10.Core;
using System.Runtime.CompilerServices;
using System;
using System.Linq.Expressions;

namespace Template10.Mvvm
{
    public abstract class ViewModelBase
        : BindableBase,
        INavigatedToAwareAsync,
        INavigatedFromAwareAsync,
        IConfirmNavigationAsync,
        ITemplate10ViewModel
    {
        public ViewModelBase()
        {
            SessionState = Central.SessionState;
        }

        /// <remarks>
        /// It is not necessary to call base.OnNavigatedToAsync in the concrete override
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async virtual Task OnNavigatedToAsync(INavigatedToParameters parameter)
        {
            await Task.CompletedTask;
        }

        public async virtual Task OnNavigatedFromAsync(INavigatedFromParameters parameters)
        {
            await Task.CompletedTask;
        }

        public async virtual Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters)
        {
            await Task.CompletedTask;
            return true;
        }

        // add helper properties

        [JsonIgnore]
        public IWindowEx Window => NavigationService.GetWindow();

        [JsonIgnore]
        public IDispatcherEx Dispatcher => Window.Dispatcher;

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public IPropertyBagEx SessionState { get; private set; }

        // remove bindable overrides

        public sealed override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
        }

        public sealed override void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.RaisePropertyChanged(propertyExpression);
        }

        public sealed override bool Set<T>(Expression<Func<T>> propertyExpression, ref T storage, T value)
        {
            return base.Set(propertyExpression, ref storage, value);
        }

        public sealed override bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            return base.Set(ref storage, value, propertyName);
        }

        // remove object overrides

        public sealed override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public sealed override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public sealed override string ToString()
        {
            return base.ToString();
        }
    }
}