using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.Storage;
using Template10.BCL;
using System.Threading.Tasks;

namespace Template10.Services.Navigation
{
    public interface ISuspensionService: IService
    {
        Task<ISuspensionState> GetStateAsync(string frameId, Type type, int backStackDepth);
    }
}