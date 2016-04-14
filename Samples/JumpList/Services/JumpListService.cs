using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Template10.Samples.JumpListSample.Services
{
    public class JumpListService
    {
        public async Task AddAsync(StorageFile file, int max = 4)
        {
            var jumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
            jumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.None;
            while (jumpList.Items.Count() > max)
            {
                jumpList.Items.RemoveAt(jumpList.Items.Count() - 1);
            }
            if (!jumpList.Items.Any(x => x.Arguments == file.Path))
            {
                var item = Windows.UI.StartScreen.JumpListItem.CreateWithArguments(file.Path, file.DisplayName);
                jumpList.Items.Add(item);
            }
            await jumpList.SaveAsync();
        }
    }
}
