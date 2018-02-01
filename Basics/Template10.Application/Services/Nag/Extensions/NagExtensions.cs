using System;
using Prism.Windows.Services.Nag;

namespace Prism.Windows.Extensions
{
    public static class NagExtensions
    {
        internal static Prism.Windows.Services.FileService.StorageStrategies ToFileServiceStrategy(this Prism.Windows.Services.Nag.NagStorageStrategies strategy)
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
