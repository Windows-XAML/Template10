using System;

namespace Windows.Services.NagService.Extensions
{
    public static class Extensions
    {
        public static Template10.Services.FileService.StorageStrategies ToFileServiceStrategy(this Template10.Services.NagService.Nag.StorageStrategies strategy)
        {
            switch (strategy)
            {
                case Template10.Services.NagService.Nag.StorageStrategies.Local:
                    return Template10.Services.FileService.StorageStrategies.Local;
                case Template10.Services.NagService.Nag.StorageStrategies.Roaming:
                    return Template10.Services.FileService.StorageStrategies.Roaming;
                case Template10.Services.NagService.Nag.StorageStrategies.Temporary:
                    return Template10.Services.FileService.StorageStrategies.Temporary;
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
