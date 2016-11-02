using System;
using System.Threading.Tasks;

namespace Template10.Services.Navigation
{

    public class SuspensionService : ISuspensionService
    {
        public static SuspensionService Instance { get; } = new SuspensionService();
        private SuspensionService()
        {
            // private constructor
        }

        public async Task<ISuspensionState> GetStateAsync(string frameId, Type type, int backStackDepth)
        {
            // async for future-use
            return new SuspensionState(frameId, type, backStackDepth);
        }
    }
}