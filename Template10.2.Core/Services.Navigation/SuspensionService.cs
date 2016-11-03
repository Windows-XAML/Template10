using System;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{
    public class SuspensionService : ISuspensionService
    {
        public static SuspensionService Instance { get; } = new SuspensionService();
        private SuspensionService()
        {
            // private constructor
        }

    }
}