using System;
using System.Threading.Tasks;

namespace Template10.Services.StateService
{
    public enum StateServiceContainerTypes { Settings, FileSystem }

    public static class StateService
    {
        public static async Task<IStateContainer> GetAsync(string key, StateServiceContainerTypes strategy = StateServiceContainerTypes.FileSystem)
        {
            switch (strategy)
            {
                case StateServiceContainerTypes.Settings:
                    return await SettingsStateContainer.CreateAsync(SettingsStateLocations.Local, key);
                case StateServiceContainerTypes.FileSystem:
                    return await FoldersStateContainer.CreateAsync(FoldersStateLocations.Local, key);
                default:
                    throw new NotSupportedException(strategy.ToString());
            }
        }
    }
}
