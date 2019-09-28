using System;
using System.Threading.Tasks;
using Template10.Services.Dialog;
using Template10.Services.File;

namespace Template10.Services.Nag
{
    /// <summary>
    /// <see cref="INagService"/>
    /// </summary>
    public sealed class NagService : INagService
    {
        readonly NagServiceHelper _nagHelper;

        public NagService(IDialogService dialogService, IFileService fileService)
        {
            _nagHelper = new NagServiceHelper(dialogService, fileService);
        }

        public async Task DeleteResponse(string nagId, NagStorageStrategies location = NagStorageStrategies.Local)
            => await _nagHelper.Delete(nagId, location);

        public async Task<bool> ResponseExists(string nagId, NagStorageStrategies location = NagStorageStrategies.Local)
            => await _nagHelper.Exists(nagId, location);

        public async Task<NagResponseInfo> GetResponse(string nagId, NagStorageStrategies location = NagStorageStrategies.Local)
            => await _nagHelper.Load(nagId, location);

        public async Task Register(NagEx nag, TimeSpan duration)
            => await _nagHelper.Register(nag, duration);

        public async Task Register(NagEx nag, int launches)
            => await _nagHelper.Register(nag, launches);

        public async Task Register(NagEx nag, int launches, TimeSpan duration)
            => await _nagHelper.Register(nag, launches, duration);
    }
}