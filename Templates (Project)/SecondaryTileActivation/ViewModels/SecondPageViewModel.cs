using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.AdaptiveTiles;
using Template10.Services.AdaptiveTiles.Model;
using Template10.Services.SecondaryTileService;

namespace SecondaryTileActivation.ViewModels
{
    public class SecondPageViewModel : ViewModelBase
    {
        private const string PinMe = "Pin Me!";
        private const string UnPinMe = "Un-pin Me!";
        private const string TileId = "DemoAppID";
        private readonly SecondaryTileHelper _secondaryTileHelper;
        private object _navigationParameter;
        private string _pinMeText;

        public SecondPageViewModel()
        {
            _secondaryTileHelper = new SecondaryTileHelper();
            UpdatePinMeText();
        }

        public string PinMeText
        {
            get { return _pinMeText; }
            set { Set(ref _pinMeText, value); }
        }

        public object NavigationParameter
        {
            get { return _navigationParameter; }
            set { Set(ref _navigationParameter, value); }
        }

        private void UpdatePinMeText()
        {
            PinMeText = _secondaryTileHelper.Exists(TileId) ? UnPinMe : PinMe;
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
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
                tileInfo.VisualElements.ForegroundText = ForegroundText.Light;
                tileInfo.VisualElements.ShowNameOnSquare150x150Logo = true;
                tileInfo.VisualElements.Square150x150Logo = new Uri("ms-appx:///assets/Square150x150Logo.png");
                await _secondaryTileHelper.PinAsync(
                    tileInfo,
                    TileId,
                    "Secondary Tile Launch");
            }


            UpdatePinMeText();
        }

        public void UpdatePrimaryTile()
        {
            var adaptiveTileHelper = new AdaptiveTileHelper();
            adaptiveTileHelper.UpdatePrimaryTile(CreateTile());
        }

        private Tile CreateTile()
        {
            var tile = new Tile
            {
                Visual = new Visual
                {
                    Bindings = new List<Binding>
                    {
                        new Binding
                        {
                            HintTextStacking = VisualHintTextStacking.Center,
                            Branding = VisualBranding.None,
                            Template = "TileSmall",
                            Children = new List<IVisualChild>
                            {
                                new Text
                                {
                                    HintStyle = TextStyle.Caption,
                                    Content = "TilesRTM"
                                }
                            }
                        },
                        new Binding
                        {
                            DisplayName = "Refactored",
                            Template = "TileMedium",
                            Branding = VisualBranding.Name,
                            Children = new List<IVisualChild>
                            {
                                new Text
                                {
                                    HintStyle = TextStyle.Caption,
                                    Content = "9:50 AM, Wednesday"
                                },
                                new Text
                                {
                                    HintStyle = TextStyle.CaptionSubtle,
                                    HintWrap = true,
                                    Content = "263 Grove St, San Francisco, CA 94102"
                                }
                            }
                        },
                        new Binding
                        {
                            DisplayName = "Refactored",
                            Template = "TileWide",
                            Children = new List<IVisualChild>
                            {
                                new Group
                                {
                                    SubGroups = new List<SubGroup>
                                    {
                                        new SubGroup
                                        {
                                            HintWeight = 33,
                                            Children = new List<ISubGroupChild>
                                            {
                                                new Image
                                                {
                                                    Placement = ImagePlacement.Inline,
                                                    Source = "ms-appx:///Assets/ToDo.png"
                                                }
                                            }
                                        },
                                        new SubGroup
                                        {
                                            Children = new List<ISubGroupChild>
                                            {
                                                new Text
                                                {
                                                    HintStyle = TextStyle.Caption,
                                                    Content = "9:50 AM, Wednesday"
                                                },
                                                new Text
                                                {
                                                    HintStyle = TextStyle.CaptionSubtle,
                                                    HintWrap = true,
                                                    HintMaxLines = 3,
                                                    Content = "263 Grove St, San Francisco, CA 94102"
                                                }
                                            }
                                            }
                                        }
                                    }
                            }
                        },
                        new Binding
                        {
                            Template = "TileLarge",
                            DisplayName = "Refactored",
                            Children = new List<IVisualChild>
                            {
                                new Group
                                {
                                    SubGroups = new List<SubGroup>
                                    {
                                        new SubGroup
                                        {
                                            HintWeight = 33,
                                            Children = new List<ISubGroupChild>
                                            {
                                                new Image
                                                {
                                                    Placement = ImagePlacement.Inline,
                                                    Source = "ms-appx:///Assets/ToDo.png"
                                                }
                                            }
                                        },
                                        new SubGroup
                                        {
                                            Children = new List<ISubGroupChild>
                                            {
                                                new Text
                                                {
                                                    HintStyle = TextStyle.Caption,
                                                    Content = "9:50 AM, Wednesday"
                                                },
                                                new Text
                                                {
                                                    HintStyle = TextStyle.CaptionSubtle,
                                                    HintWrap = true,
                                                    HintMaxLines = 3,
                                                    Content = "263 Grove St, San Francisco, CA 94102"
                                                }
                                            }
                                            }
                                    }
                                },
                                new Image
                                {
                                    Placement = ImagePlacement.Inline,
                                    Source = "ms-appx:///Assets/banner.png"
                                }
                            }
                        }
                    }
                }
            };

            return tile;
        }
    }
}
