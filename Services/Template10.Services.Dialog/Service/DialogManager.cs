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
        static SemaphoreSlim _OnlyOneAsync = new SemaphoreSlim(1, 1);
        internal static async Task<T> OnlyOneAsync<T>(Func<Task<T>> show)
        {
            await _OnlyOneAsync.WaitAsync();
            try
            {
                return await show();
            }
            finally
            {
                _OnlyOneAsync.Release();
            }
        }
    }
}
