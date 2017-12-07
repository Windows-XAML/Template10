using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var to = timeout ?? TimeSpan.FromHours(1);
            var tk = token ?? new CancellationToken(false);
            if (!await _OneAtATimeAsync.WaitAsync(to, tk))
            {
                throw new Exception($"{nameof(DialogManager)}.{nameof(OneAtATimeAsync)} has timed out.");
            }
            try { return await show(); }
            finally { _OneAtATimeAsync.Release(); }
        }
    }
}
