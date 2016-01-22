using System.Collections.Generic;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Sample.ViewModels
{
    public class MainPageViewModel : Mvvm.ViewModelBase
    {
        public MainPageViewModel()
        {
            if (base.IsInDesignMode)
                Value = "Designtime value";
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            Value = parameter?.ToString();
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set { Set(ref _Value, value); }
        }


        DelegateCommand _GotoDetailsCommand;
        public DelegateCommand GotoDetailsCommand
           => _GotoDetailsCommand ?? (_GotoDetailsCommand = new DelegateCommand(GotoDetailsCommandExecute, GotoDetailsCommandCanExecute));
        bool GotoDetailsCommandCanExecute() => true;
        void GotoDetailsCommandExecute()
        {
            NavigationService.Navigate(typeof(Views.DetailPage));
        }
    }
}
