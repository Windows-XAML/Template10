using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template10.App
{
    public interface IBootstrapper
    {
        Task InitializeAsync();
        Task<bool> PrelaunchAsync();
        Task StartAsync();
    }

    public abstract partial class Bootstrapper : Windows.UI.Xaml.Application, IBootstrapper
    {
        public virtual async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public virtual async Task PrelaunchAsync(bool stop)
        {
            stop = false;
            await Task.CompletedTask;
        }

        public abstract Task StartAsync();
    }
}
