using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.NavigationService
{
    class CachingService
    {
        public void Clear(Frame frame, bool removeCachedPagesInBackStack)
        {
            int currentSize = frame.CacheSize;
            if (removeCachedPagesInBackStack)
            {
                frame.CacheSize = 0;
            }
            else
            {
                if (frame.BackStackDepth == 0)
                    frame.CacheSize = 1;
                else
                    frame.CacheSize = frame.BackStackDepth;
            }
            frame.CacheSize = currentSize;
        }
    }
}
