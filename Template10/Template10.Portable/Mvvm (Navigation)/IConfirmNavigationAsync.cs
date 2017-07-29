using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Navigation
{
    public interface IConfirmNavigationAsync
    {
        Task<bool> CanNavigateAsync(IConfirmNavigationParameters parameters);
    }
}
