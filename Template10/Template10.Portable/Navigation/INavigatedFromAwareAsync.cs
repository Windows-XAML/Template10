using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Template10.Portable.Navigation
{
    public interface INavigatedFromAwareAsync
    {
        Task OnNavigatedFromAsync(INavigatedFromParameters parameters);
    }
}
