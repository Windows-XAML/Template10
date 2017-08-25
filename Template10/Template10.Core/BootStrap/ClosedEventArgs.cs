using System;
using System.Threading.Tasks;

namespace Template10.BootStrap
{
    public class ClosedEventArgs : EventArgs
    {
        Func<Action, Task<bool>> _tryToExtendAsync;
        public ClosedEventArgs(Func<Action, Task<bool>> tryToExtendAsync)
            => _tryToExtendAsync = tryToExtendAsync;
        public async Task<bool> TryToExtendAsync()
            => await _tryToExtendAsync?.Invoke(null);
        public async Task<bool> TryToExtendAsync(Action revoked)
            => await _tryToExtendAsync?.Invoke(revoked);
    }
}
