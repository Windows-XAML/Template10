using System;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Navigation
{
    public class ViewModelLogic : IViewModelLogic
    {
        public static IViewModelLogic Instance { get; set; } = new ViewModelLogic();
        private ViewModelLogic()
        {
            // private constructor
        }

        public virtual object ResolveViewModel(Page page)
        {
            this.DebugWriteInfo();

            return page?.DataContext;
        }

        public virtual object ResolveViewModel(Type page)
        {
            this.DebugWriteInfo();

            return null;
        }
    }
}