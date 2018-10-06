using System;
using System.Threading.Tasks;

using Template10.Services.File;

namespace Template10.Services.Nag
{
    /// <summary>
    /// Interface for service that will nag the user to perform some action
    /// (such as reviewing an app) after a certain number of launches and/or time has
    /// past since the first launch
    /// </summary>
    public interface INagService
    {
        /// <summary>
        /// Registers and checks whether to display a nag
        /// </summary>
        /// <param name="nag">The <see cref="NagEx"/></param>
        /// <param name="launches">The number of app launches to pass before showing the nag</param>
        /// <param name="duration">The amount of time to pass from the first launch before showing the nag</param>
        /// <returns><see cref="Task"/></returns>
        Task Register(NagEx nag, int launches, TimeSpan duration);

        /// <summary>
        /// Registers and checks whether to display a nag
        /// </summary>
        /// <param name="nag">The <see cref="NagEx"/></param>
        /// <param name="launches">The number of app launches to pass before showing the nag</param>
        /// <returns><see cref="Task"/></returns>
        Task Register(NagEx nag, int launches);

        /// <summary>
        /// Registers and checks whether to display a nag
        /// </summary>
        /// <param name="nag">The <see cref="NagEx"/></param>
        /// <param name="duration">The amount of time to pass from the first launch before showing the nag</param>
        /// <returns><see cref="Task"/></returns>
        Task Register(NagEx nag, TimeSpan duration);

        /// <summary>
        /// Determines if the given nag has been registered
        /// </summary>
        /// <param name="nagId">The if of the <see cref="NagEx"/></param>
        /// <returns>True if a <see cref="NagResponseInfo"/> exists for the nagId</returns>
        Task<bool> ResponseExists(string nagId, NagStorageStrategies location = NagStorageStrategies.Local);

        /// <summary>
        /// Gets the <see cref="NagResponseInfo"/> for the given id
        /// </summary>
        /// <param name="nagId">The if of the <see cref="NagEx"/> to find</param>
        /// <returns>A <see cref="NagResponseInfo"/> or null if it doesn't exist</returns>
        Task<NagResponseInfo> GetResponse(string nagId, NagStorageStrategies location = NagStorageStrategies.Local);

        /// <summary>
        /// Deletes persisted <see cref="NagResponseInfo"/>
        /// </summary>
        /// <param name="nagId">The id of the <see cref="NagEx"/> to delete</param>
        /// <returns><see cref="Task"/></returns>
        Task DeleteResponse(string nagId, NagStorageStrategies location = NagStorageStrategies.Local);
    }
}
