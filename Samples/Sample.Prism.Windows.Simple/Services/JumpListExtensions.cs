using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Sample.Services
{
    public static class JumpListExtensions
    {
        public static async Task<bool> TrySaveAsync(this JumpList jumpList)
        {
            try
            {
                await jumpList.SaveAsync();
                return true;
            }
            catch { return false; }
        }

        public static void TrimItems(this JumpList jumpList, int max)
        {
            foreach (var item in jumpList.Items.Skip(max).ToArray())
            {
                jumpList.Items.Remove(item);
            }
        }

        public static JumpListItem InsertItem(this JumpList jumpList, string argument,
            string display, string image, string group, string discription)
        {
            var item = JumpListItem.CreateWithArguments(argument, display);
            item.Description = discription;
            item.GroupName = group;
            if (image != null)
            {
                item.Logo = new Uri(image);
            }
            jumpList.Items.Insert(0, item);
            return item;
        }

        public static void RemoveExisting(this JumpList jumpList, string display)
        {
            foreach (var item in jumpList.Items
                .Where(x => Equals(x.DisplayName.ToLower(), display.ToLower())).ToArray())
            {
                jumpList.Items.Remove(item);
            }
        }
    }
}
