using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Template10.Views
{
    public sealed partial class Page2 : Page
    {
        public Page2()
        {
            InitializeComponent();
            DataContextChanged += (s, e) => { ViewModel = DataContext as Page2ViewModel; };
        }

        public Page2ViewModel ViewModel { get; set; }
    }

    public class Page2ViewModel : Mvvm.ViewModelBase
    {
        public Page2ViewModel()
        {
            Value = "Constructor";
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.ContainsKey("Value"))
                Value = state["Value"].ToString();
            else
                Value = parameter?.ToString() ?? "Empty";
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
                state["Value"] = "Suspended " + DateTime.Now.ToString();
            else
                state["Value"] = "Navigated " + DateTime.Now.ToString();
        }

        public override void OnNavigatingFrom(NavigatingEventArgs args)
        {
            args.Cancel = false;
        }

        private string _Value = default(string);
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }
    }
}
