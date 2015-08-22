using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Template10.ViewModels
{
    class Locator
    {
        Mvvm.IoC.IContainer _container;

        public Locator()
        {
            _container = new Mvvm.IoC.Container();
        }

        public void Initialize()
        {
            // services
            _container.Register<Services.NavigationService.NavigationService>((App.Current as Common.BootStrapper).NavigationService);
            _container.Register<Services.DispatcherService.DispatcherService>(new Services.DispatcherService.DispatcherService(Window.Current.Dispatcher));

            // repositories
            _container.Register<Repositories.ColorRepository, Repositories.ColorRepository>();
            _container.Register<Repositories.FavoritesRepository, Repositories.FavoritesRepository>();
        }

        public ViewModels.MainPageViewModel MainPageViewModel
        {
            get { return _container.Resolve<ViewModels.MainPageViewModel>(); }
        }
    }
}
