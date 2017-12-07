using System;
using System.Threading.Tasks;
using Template10.Common;

namespace Template10.Navigation
{
    public interface IFrameExState
    {
        Task ClearAsync();
        Task SetCurrentPageParameterAsync(object value);
        Task SetCurrentPageTypeAsync(Type value);
        Task SetNavigationStateAsync(string value);
        Task<TryResult<object>> TryGetCurrentPageParameterAsync();
        Task<TryResult<Type>> TryGetCurrentPageTypeAsync();
        Task<TryResult<string>> TryGetNavigationStateAsync();
    }
}