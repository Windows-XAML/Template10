using System;
using System.Threading.Tasks;
using Template10.Services.DialogService;
using Template10.Services.FileService;
using Windows.Services.NagService.Extensions;
using Windows.UI.Popups;

namespace Template10.Services.NagService
{
    public class NagServiceHelper
    {
        readonly IDialogService _dialogService;
        readonly IFileService _fileService;

        const string StateFileNameTemplate = "Template10.Services.NagService.{0}.json";

        public NagServiceHelper(IDialogService dialogService, IFileService fileService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException("dialogService");
            _fileService = fileService ?? throw new ArgumentNullException("fileService");
        }

        public async Task<bool> Exists(string nagId, StorageStrategies location)
        {
            if (string.IsNullOrEmpty(nagId)) throw new ArgumentException("nagId cannot be null or empty", "nagId");

            return await _fileService.FileExistsAsync(string.Format(StateFileNameTemplate, nagId));
        }

        public async Task<NagResponseInfo> Load(string nagId, StorageStrategies location)
        {
            if (string.IsNullOrEmpty(nagId)) throw new ArgumentException("nagId cannot be null or empty", "nagId");

            try
            {
                if (await Exists(nagId, location))
                {
                    return await _fileService.ReadFileAsync<NagResponseInfo>(GetFileName(nagId));
                }

                return new NagResponseInfo() { NagId = nagId };
            }
            catch
            {
                // if we are having trouble loading the response, suppress any nags
                // this should prevent nagging repeatedly because of an unknown error 
                return new NagResponseInfo() { Suppress = true };
            }
        }

        public async Task Delete(string nagId, StorageStrategies location)
        {
            if (string.IsNullOrEmpty(nagId)) throw new ArgumentException("nagId cannot be null or empty", "nagId");

            await _fileService.DeleteFileAsync(GetFileName(nagId));
        }

        public async Task Persist(NagResponseInfo state, string nagId, StorageStrategies location)
        {
            if (state == null) throw new ArgumentNullException("state");
            if (string.IsNullOrEmpty(nagId)) throw new ArgumentException("nagId cannot be null or empty", "nagId");

            await _fileService.WriteFileAsync<NagResponseInfo>(state, GetFileName(nagId));
        }

        public async Task Register(Nag nag, int launches)
        {
            if (nag == null) throw new ArgumentNullException("nag");

            var responseInfo = await Load(nag.Id, nag.Location.ToFileServiceStrategy());
            responseInfo.LaunchCount++;

            if (responseInfo.ShouldNag(launches))
            {
                await ProcessNag(nag, responseInfo);
            }
            else if (responseInfo.IsAwaitingResponse)
            {
                await Persist(responseInfo, nag.Id, nag.Location.ToFileServiceStrategy());
            }
        }

        public async Task Register(Nag nag, TimeSpan duration)
        {
            if (nag == null) throw new ArgumentNullException("nag");

            var responseInfo = await Load(nag.Id, nag.Location.ToFileServiceStrategy());
            responseInfo.LaunchCount++;

            if (responseInfo.ShouldNag(duration))
            {
                await ProcessNag(nag, responseInfo);
            }
            else if (responseInfo.IsAwaitingResponse)
            {
                await Persist(responseInfo, nag.Id, nag.Location.ToFileServiceStrategy());
            }
        }

        public async Task Register(Nag nag, int launches, TimeSpan duration)
        {
            if (nag == null) throw new ArgumentNullException("nag");

            var responseInfo = await Load(nag.Id, nag.Location.ToFileServiceStrategy());
            responseInfo.LaunchCount++;

            if (responseInfo.ShouldNag(launches) && responseInfo.ShouldNag(duration))
            {
                await ProcessNag(nag, responseInfo);
            }
            else if (responseInfo.IsAwaitingResponse)
            {
                await Persist(responseInfo, nag.Id, nag.Location.ToFileServiceStrategy());
            }
        }

        private async Task ProcessNag(Nag nag, NagResponseInfo responseInfo)
        {
            var response = await ShowNag(nag);

            if (response == NagResponse.Accept)
            {
                nag.NagAction();
            }
            else if (response == NagResponse.Defer)
            {
                responseInfo.LaunchCount = 0;
                responseInfo.RegistrationTimeStamp = DateTimeOffset.UtcNow;
            }

            responseInfo.LastResponse = response;
            responseInfo.LastNag = DateTimeOffset.UtcNow;

            await Persist(responseInfo, nag.Id, nag.Location.ToFileServiceStrategy());
        }

        private async Task<NagResponse> ShowNag(Nag nag)
        {
            var response = NagResponse.NoResponse;
            if (nag.AllowDefer)
            {
                await _dialogService.ShowAsync(nag.Message, nag.Title,
                    new UICommand(nag.AcceptText, command => response = NagResponse.Accept),
                    new UICommand(nag.DeclineText, command => response = NagResponse.Decline),
                    new UICommand(nag.DeferText, command => response = NagResponse.Defer));
            }
            else
            {
                await _dialogService.ShowAsync(nag.Message, nag.Title,
                    new UICommand(nag.AcceptText, command => response = NagResponse.Accept),
                    new UICommand(nag.DeclineText, command => response = NagResponse.Decline));
            }

            return response;
        }

        private static string GetFileName(string nagId)
        {
            return string.Format(StateFileNameTemplate, nagId);
        }
    }
}
