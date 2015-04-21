using Template10.Services.ColorService;
using System;
using Template10.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace Template10.ViewModels
{
    public class DetailPageViewModel : Mvvm.ViewModelBase
    {
        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                LoadDesignData();
        }

        private void LoadDesignData()
        {
            this.ColorInfo = new Models.ColorInfo
            {
                Name = "Orange",
                Color = Colors.Orange,
            };
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, Dictionary<string, object> state)
        {
            LoadRuntimeData(parameter);
        }

        private async void LoadRuntimeData(string parameter)
        {
            var repository = new Repositories.ColorRepository();
            this.ColorInfo = await repository.GetColorAsync(parameter);
        }

        private Models.ColorInfo _colorInfo;
        public Models.ColorInfo ColorInfo
        {
            get { return _colorInfo; }
            set
            {
                Set(ref _colorInfo, value);
                if (value != null)
                    Setup();
            }
        }

        void Setup()
        {
            var triad = this.ColorInfo.Color.GetTriads().ToArray();
            this.Variant1 = new SolidColorBrush(triad[0]);
            this.Variant2 = new SolidColorBrush(triad[1]);
            this.Variant3 = new SolidColorBrush(triad[2]);
        }

        public ObservableCollection<SolidColorBrush> Shades { get; } = new ObservableCollection<SolidColorBrush>();
        public ObservableCollection<SolidColorBrush> Triads { get; } = new ObservableCollection<SolidColorBrush>();
        public ObservableCollection<SolidColorBrush> Tetradicts { get; } = new ObservableCollection<SolidColorBrush>();
        public ObservableCollection<SolidColorBrush> Chromatics { get; } = new ObservableCollection<SolidColorBrush>();

        private SolidColorBrush _variant1;
        public SolidColorBrush Variant1 { get { return _variant1; } set { Set(ref _variant1, value); } }

        private SolidColorBrush _variant2;
        public SolidColorBrush Variant2 { get { return _variant2; } set { Set(ref _variant2, value); } }

        private SolidColorBrush _variant3;
        public SolidColorBrush Variant3 { get { return _variant3; } set { Set(ref _variant3, value); } }

        public Mvvm.Command GoBackCommand
        {
            get { return new Mvvm.Command(() => { (App.Current as Common.BootStrapper).NavigationService.GoBack(); }); }
        }
    }
}
