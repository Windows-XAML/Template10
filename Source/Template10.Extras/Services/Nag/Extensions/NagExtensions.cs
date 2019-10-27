using System;
using Template10.Services.Nag;

namespace Template10.Extensions
{
    public static class NagExtensions
    {
        internal static Template10.Services.File.StorageStrategies ToFileServiceStrategy(this Template10.Services.Nag.NagStorageStrategies strategy)
        {
            switch (strategy)
            {
                case NagStorageStrategies.Local: return Services.File.StorageStrategies.Local;
                case NagStorageStrategies.Roaming: return Services.File.StorageStrategies.Roaming;
                case NagStorageStrategies.Temporary: return Services.File.StorageStrategies.Temporary;
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
