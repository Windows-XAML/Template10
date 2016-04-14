using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Samples.TilesSample.ViewModels;
using Windows.UI.StartScreen;
using Windows.UI.Notifications;
using NotificationsExtensions.Tiles;
using NotificationsExtensions.Toasts;

namespace Template10.Samples.TilesSample.Services.TileService
{
    class TileService
    {
        internal async Task<bool> IsPinned(DetailPageViewModel detailPageViewModel)
        {
            var tileId = detailPageViewModel.ToString();
            return (await SecondaryTile.FindAllAsync()).Any(x => x.TileId.Equals(tileId));
        }

        internal async Task<bool> PinAsync(DetailPageViewModel detailPageViewModel)
        {
            var name = "Tiles sample";
            var title = "Template 10";
            var body = detailPageViewModel.Value;
            var image = "https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/Template10.png";

            var bindingContent = new TileBindingContentAdaptive()
            {
                PeekImage = new TilePeekImage()
                {
                    Source = new TileImageSource(image)
                },

                Children =
                {
                    new TileText()
                    {
                        Text = title,
                        Style = TileTextStyle.Body
                    },

                    new TileText()
                    {
                        Text = body,
                        Wrap = true,
                        Style = TileTextStyle.CaptionSubtle
                    }
                }
            };

            var binding = new TileBinding()
            {
                Branding = TileBranding.NameAndLogo,
                DisplayName = name,
                Content = bindingContent
            };

            var content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = binding,
                    TileWide = binding,
                    TileLarge = binding
                }
            };

            var xml = content.GetXml();

            // show tile

            var tileId = detailPageViewModel.ToString();

            if (!await IsPinned(detailPageViewModel))
            {
                // initial pin
                var logo = new Uri("ms-appx:///Assets/Logo.png");
                var secondaryTile = new SecondaryTile(tileId)
                {
                    Arguments = detailPageViewModel.Value,
                    DisplayName = name,
                    VisualElements =
                        {
                            Square44x44Logo = logo,
                            Square150x150Logo = logo,
                            Wide310x150Logo = logo,
                            Square310x310Logo = logo,
                            ShowNameOnSquare150x150Logo = true,
                        },
                };
                if (!await secondaryTile.RequestCreateAsync())
                {
                    System.Diagnostics.Debugger.Break();
                    return false;
                }
            }

            // add notifications

            var tileNotification = new TileNotification(xml);
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            tileUpdater.Update(tileNotification);

            // show toast

            ShowToast(detailPageViewModel);

            // result

            return true;
        }

        void ShowToast(DetailPageViewModel detailPageViewModel)
        {
            var data = detailPageViewModel.Value;
            var image = "https://raw.githubusercontent.com/Windows-XAML/Template10/master/Assets/Template10.png";

            var content = new ToastContent()
            {
                Launch = data,
                Visual = new ToastVisual()
                {
                    TitleText = new ToastText()
                    {
                        Text = "Secondary tile pinned"
                    },

                    BodyTextLine1 = new ToastText()
                    {
                        Text = detailPageViewModel.Value
                    },

                    AppLogoOverride = new ToastAppLogo()
                    {
                        Crop = ToastImageCrop.Circle,
                        Source = new ToastImageSource(image)
                    }
                },
                Audio = new ToastAudio()
                {
                    Src = new Uri("ms-winsoundevent:Notification.IM")
                }
            };

            var notification = new ToastNotification(content.GetXml());
            var notifier = ToastNotificationManager.CreateToastNotifier();
            notifier.Show(notification);
        }

        internal async Task<bool> UnPinAsync(DetailPageViewModel detailPageViewModel)
        {
            if (!await IsPinned(detailPageViewModel))
                return true;
            try
            {
                var tileId = detailPageViewModel.ToString();
                var tile = new SecondaryTile(tileId);
                return await tile.RequestDeleteAsync();
            }
            catch (Exception)
            {
                System.Diagnostics.Debugger.Break();
                return false;
            }
        }
    }
}
