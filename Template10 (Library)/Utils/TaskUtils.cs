using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Template10.Utils
{
    public static class TaskUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SuppressWarning(this Task task) { }
    }
}