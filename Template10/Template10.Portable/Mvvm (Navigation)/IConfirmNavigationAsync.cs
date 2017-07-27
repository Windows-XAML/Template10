using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Mvvm
{
    public interface IConfirmNavigationAsync
    {
        Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters);
    }
}
