using System;
using Template10.Service.Nag;

namespace Template10.Extensions
{
    public static class NagExtensions
    {
        internal static Template10.Service.File.StorageStrategies ToFileServiceStrategy(this Template10.Service.Nag.NagStorageStrategies strategy)
        {
            switch (strategy)
            {
                case NagStorageStrategies.Local: return Service.File.StorageStrategies.Local;
                case NagStorageStrategies.Roaming: return Service.File.StorageStrategies.Roaming;
                case NagStorageStrategies.Temporary: return Service.File.StorageStrategies.Temporary;
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
