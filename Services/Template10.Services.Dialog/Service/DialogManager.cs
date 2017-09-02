using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Template10.Services.Dialog
{
    internal static class DialogManager
    {
        static SemaphoreSlim _OneAtATimeAsync = new SemaphoreSlim(1, 1);
        internal static async Task<T> OneAtATimeAsync<T>(Func<Task<T>> show, TimeSpan? timeout, CancellationToken? token)
        {
            if (!await _OneAtATimeAsync.WaitAsync(timeout ?? TimeSpan.MaxValue, token ?? new CancellationToken(false)))
            {
                throw new Exception($"{nameof(DialogManager)}.{nameof(OneAtATimeAsync)} has timed out.");
            }
            try { return await show(); }
            finally { _OneAtATimeAsync.Release(); }
        }


    }
}
