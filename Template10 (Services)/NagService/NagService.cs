using System;
using System.Threading.Tasks;

using Template10.Services.DialogService;
using Template10.Services.FileService;

namespace Template10.Services.NagService
{
    public sealed class NagService : INagService
    {
        readonly NagServiceHelper _nagHelper;

        public NagService(IDialogService dialogService, IFileService fileService)
        {
            _nagHelper = new NagServiceHelper(dialogService, fileService);
        }

        public async Task DeleteResponse(string nagId) => await _nagHelper.Delete(nagId);

        public async Task<bool> ResponseExists(string nagId) => await _nagHelper.Exists(nagId);

        public async Task<NagResponseInfo> GetResponse(string nagId) => await _nagHelper.Load(nagId);

        public async Task Register(Nag nag, TimeSpan duration) => await _nagHelper.Register(nag, duration);

        public async Task Register(Nag nag, int launches) => await _nagHelper.Register(nag, launches);

        public async Task Register(Nag nag, int launches, TimeSpan duration) => await _nagHelper.Register(nag, launches, duration);
    }
}