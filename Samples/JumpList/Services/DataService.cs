using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Samples.JumpListSample.Services
{
    public class DataService
    {
        FutureService _FutureService;
        JumpListService _JumpListService;

        public DataService()
        {
            _FutureService = new FutureService();
            _JumpListService = new JumpListService();
        }

        public async Task SaveFileInfoAsync(Models.FileInfo model)
        {
            // nothing to save (can be okay)
            if (model == null) return;

            // save content to file
            await Windows.Storage.FileIO.WriteTextAsync(model.Ref, model.Text);
        }

        public async Task<Models.FileInfo> GetFileInfoAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var file = await StorageFile.GetFileFromPathAsync(path);
            return await GetFileInfoAsync(file);
        }

        public async Task<Models.FileInfo> GetFileInfoAsync(StorageFile file)
        {
            if (file == null)
            {
                // nothing to do
                return null;
            }
            else
            {
                // add as future item
                _FutureService.Add(file);
            }

            // add to jumplist
            await _JumpListService.AddAsync(file);

            // return model
            return new Template10.Samples.JumpListSample.Models.FileInfo
            {
                Text = await FileIO.ReadTextAsync(file),
                Name = file.DisplayName,
                Ref = file,
            };
        }
    }

}
