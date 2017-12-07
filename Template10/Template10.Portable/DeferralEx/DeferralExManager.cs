using System.Threading.Tasks;

namespace Template10.Common
{

    public sealed class DeferralExManager
    {
        int _count = 0;
        TaskCompletionSource<object> _completed = new TaskCompletionSource<object>();
        public DeferralEx GetDeferral()
        {
            System.Threading.Interlocked.Increment(ref _count);
            return new DeferralEx(() =>
            {
                var count = System.Threading.Interlocked.Decrement(ref _count);
                if (count == 0)
                {
                    _completed.SetResult(null);
                }
            });
        }

        public bool IsComplete()
        {
            return WaitForDeferralsAsync().IsCompleted;
        }

        public Task WaitForDeferralsAsync()
        {
            if (_count == 0)
            {
                return Task.CompletedTask;
            }

            return _completed.Task;
        }
    }

}