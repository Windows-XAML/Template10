using System.Threading.Tasks;
using Template10.Services.FileService;

namespace Sample.Services
{
    public class ProfileRepository : IProfileRepository
    {
        private IFileService _fileService;

        public ProfileRepository(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task SaveAsync(Models.Profile profile)
        {
            await _fileService.WriteFileAsync(BuildKey(profile.Key), profile);
        }

        public async Task<Models.Profile> LoadAsync(string key)
        {
            return await _fileService.ReadFileAsync<Models.Profile>(BuildKey(key));
        }

        string BuildKey(string key)
        {
            return $"Profile-{key}";
        }
    }
}
