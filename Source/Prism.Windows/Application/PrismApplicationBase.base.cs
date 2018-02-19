using win = Windows;
using Prism.Events;
using Prism.Ioc;
using Prism.Logging;
using Prism.Navigation;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace Prism.Windows
{
    public interface IPrismApplicationBase
    {
        void ConfigureViewModelLocator();
        IContainerExtension CreateContainer();
        void RegisterRequiredTypes(IContainerRegistry container);
        void RegisterTypes(IContainerRegistry container);
        void OnInitialize();
        void OnStart(StartArgs args);
        Task OnStartAsync(StartArgs args);
    }

    public abstract partial class PrismApplicationBase : IPrismApplicationBase
    {
        public new static PrismApplicationBase Current => (PrismApplicationBase)Application.Current;

        private static SemaphoreSlim _startSemaphore = new SemaphoreSlim(1, 1);

        public PrismApplicationBase()
        {
            InternalInitialize();
        }

        public virtual void ConfigureViewModelLocator()
        {
            Prism.Mvvm.ViewModelLocationProvider.SetDefaultViewModelFactory((view, type) =>
            {
                return Container.Resolve(type);
            });
        }

        IContainerExtension _containerExtension;
        public IContainerProvider Container => _containerExtension;

        public abstract IContainerExtension CreateContainer();

        public abstract void RegisterRequiredTypes(IContainerRegistry container);

        public abstract void RegisterTypes(IContainerRegistry container);

        public virtual void OnInitialize() { /* empty */ }

        public virtual void OnStart(StartArgs args) {  /* empty */ }

        public virtual Task OnStartAsync(StartArgs args) => Task.CompletedTask;

        private void InternalInitialize()
        {
            Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalInitialize)}");
            _containerExtension = CreateContainer();
            RegisterRequiredTypes(_containerExtension as IContainerRegistry);
            RegisterTypes(_containerExtension as IContainerRegistry);
            _containerExtension.FinalizeExtension();
            ConfigureViewModelLocator();
            OnInitialize();
        }

        private async Task InternalStartAsync(StartArgs startArgs)
        {
            await _startSemaphore.WaitAsync();
            Debug.WriteLine($"{nameof(PrismApplicationBase)}.{nameof(InternalStartAsync)}({startArgs})");
            try
            {
                Window.Current.Activate();
                OnStart(startArgs);
                await OnStartAsync(startArgs);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR {ex.Message}");
                Debugger.Break();
            }
            finally
            {
                _startSemaphore.Release();
            }
        }
    }
}
