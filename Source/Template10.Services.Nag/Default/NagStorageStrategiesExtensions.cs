using System;
using Template10.Services;

namespace Template10
{
    public static class NagExtensions
    {
        internal static StorageStrategies ToFileServiceStrategy(this NagStorageStrategies strategy)
        {
            switch (strategy)
            {
                case NagStorageStrategies.Local: return StorageStrategies.Local;
                case NagStorageStrategies.Roaming: return StorageStrategies.Roaming;
                case NagStorageStrategies.Temporary: return StorageStrategies.Temporary;
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
