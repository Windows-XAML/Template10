using System;
using System.Threading.Tasks;
using Template10.Portable.State;

namespace Template10.Services.StateService
{
    public enum StateServiceContainerTypes { Settings, FileSystem }

    public static class StateService
    {
        public static SerializationService.ISerializationService DefaultSerializationStrategy { get; set; } = SerializationService.SerializationService.Json;

        public static async Task<IPersistedStateContainer> GetAsync(string key, string container = null, StateServiceContainerTypes type = StateServiceContainerTypes.FileSystem)
        {
            switch (type)
            {
                case StateServiceContainerTypes.Settings:
                    return await SettingsStateContainer.CreateAsync(SettingsStateLocations.Local, container, key);
                case StateServiceContainerTypes.FileSystem:
                    return await FoldersStateContainer.CreateAsync(FoldersStateLocations.Local, container, key);
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }
    }
}
