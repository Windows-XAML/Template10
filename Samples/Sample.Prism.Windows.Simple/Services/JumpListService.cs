using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Sample.Services
{
    public class JumpListService : IJumpListService
    {

        public async Task<bool> AddAsync(string argument, string display,
            string image = null, int max = 5, JumpListSystemGroupKind kind = JumpListSystemGroupKind.Recent)
        {
            if (JumpList.IsSupported())
            {
                var jumpList = await JumpList.LoadCurrentAsync();
                jumpList.SystemGroupKind = kind;
                jumpList.RemoveExisting(display);
                jumpList.InsertItem(argument, display, image, string.Empty, string.Empty);
                jumpList.TrimItems(max);
                return await jumpList.TrySaveAsync();
            }
            else
            {
                return false;
            }
        }

    }
}
