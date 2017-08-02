using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Mvvm
{
    public interface INavigatedFromAwareAsync
    {
        Task OnNavigatedFromAsync(INavigatedFromParameters parameters);
    }
}
