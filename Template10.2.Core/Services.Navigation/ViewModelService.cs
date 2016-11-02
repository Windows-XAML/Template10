using System;
using System.Threading.Tasks;
using Template10.BCL;
using Template10.Services.Lifetime;
using Template10.Services.Serialization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace Template10.Services.Navigation
{
    public class ViewModelService : IViewModelService
    {
        public static ViewModelService Instance { get; } = new ViewModelService();
        private ViewModelService()
        {
            // private constructor
        }

        public object ResolveForPage(Page page) => page?.DataContext;
    }
}
