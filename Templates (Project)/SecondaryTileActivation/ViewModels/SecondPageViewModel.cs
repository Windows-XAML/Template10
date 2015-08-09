using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SecondaryTileService;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace SecondaryTileActivation.ViewModels
{
    public class SecondPageViewModel : ViewModelBase
    {
        private string _navigationParameter;
        private string _pinMeText;
        private const string PinMe = "Pin Me!";
        private const string UnPinMe = "Un-pin Me!";
        private const string TileId = "DemoAppID";
        private SecondaryTileHelper _secondaryTileHelper;

        public string PinMeText
        {
            get { return _pinMeText; }
            set { Set(ref _pinMeText, value); }
        }

        public string NavigationParameter
        { 
            get
            {
                return _navigationParameter;
            } 
            set
            {
                Set(ref _navigationParameter, value);
            }
        }

        public SecondPageViewModel()
        {
            _secondaryTileHelper = new SecondaryTileHelper();
            UpdatePinMeText();
        }

        private void UpdatePinMeText()
        {
            PinMeText = _secondaryTileHelper.Exists(TileId) ? UnPinMe : PinMe;
        }

        public override void OnNavigatedTo(string parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            NavigationParameter = parameter;
            base.OnNavigatedTo(parameter, mode, state);
        }

        public async void PinSecondaryTile()
        {
            if (_secondaryTileHelper.Exists(TileId))
            {
                await _secondaryTileHelper.UnpinAsync(TileId);
            }
            else
            {
                var tileInfo = new SecondaryTileHelper.TileInfo
                {
                    DisplayName = "My Secondary Tile",
                    PhoneticName = "My Secondary Tile"
                };
                tileInfo.VisualElements.BackgroundColor = Colors.Green;
                tileInfo.VisualElements.ForegroundText = Windows.UI.StartScreen.ForegroundText.Light;
                tileInfo.VisualElements.ShowNameOnSquare150x150Logo = true;
                tileInfo.VisualElements.Square150x150Logo = new Uri("ms-appx:///assets/Square150x150Logo.png");
                await _secondaryTileHelper.PinAsync(
                    tileInfo,
                    TileId,
                    "Secondary Tile Launch");
            }


            UpdatePinMeText();
        }
    }
}
