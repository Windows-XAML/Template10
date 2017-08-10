using System;
using Template10.Services.Nag;

namespace Template10.Extensions
{
    public static class NagExtensions
    {
        internal static Template10.Services.FileService.StorageStrategies ToFileServiceStrategy(this Template10.Services.Nag.NagStorageStrategies strategy)
        {
            switch (strategy)
            {
                case NagStorageStrategies.Local: return Services.FileService.StorageStrategies.Local;
                case NagStorageStrategies.Roaming: return Services.FileService.StorageStrategies.Roaming;
                case NagStorageStrategies.Temporary: return Services.FileService.StorageStrategies.Temporary;
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
