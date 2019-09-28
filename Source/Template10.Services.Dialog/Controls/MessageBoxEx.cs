using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Template10.Services.Dialog
{

    /// <summary>
    /// This class encapsulates the behavior for a simple message box.
    /// </summary>
    public partial class MessageBoxEx : IMessageBoxEx
    {
        public ElementTheme RequestedTheme { get; set; } = ElementTheme.Light;

        public MessageBoxEx(string title, string text, MessageBoxType messageBoxType = MessageBoxType.Ok, IDialogResourceResolver resolver = null)
        {
            Text = text;
            Title = title;
            Type = messageBoxType;
            Resolver = resolver ?? Settings.DefaultResolver;
        }

        public string Text { get; set; }
        public string Title { get; set; }
        public MessageBoxType Type { get; set; }
        public IDialogResourceResolver Resolver { get; set; }

        public async Task<MessageBoxResult> ShowAsync(TimeSpan? timeout = null, CancellationToken? token = null)
        {
            var dialog = new ContentDialog
            {
                Title = Title,
                Content = Text,
                RequestedTheme = RequestedTheme,
            };
            SetupButtons(dialog);
            var result = await DialogManager.OneAtATimeAsync(async () => await dialog.ShowAsync(), timeout, token);
            return DetermineResult(result);
        }

        private MessageBoxResult DetermineResult(ContentDialogResult result)
        {
            switch (result)
            {
                case ContentDialogResult.None:
                    return MessageBoxResult.Cancel;
                case ContentDialogResult.Primary:
                    switch (Type)
                    {
                        case MessageBoxType.Ok: return MessageBoxResult.Ok;
                        case MessageBoxType.OkCancel: return MessageBoxResult.Ok;
                        case MessageBoxType.YesNo: return MessageBoxResult.Yes;
                        case MessageBoxType.YesNoCancel: return MessageBoxResult.Yes;
                        default: return MessageBoxResult.Cancel;
                    }
                case ContentDialogResult.Secondary:
                    switch (Type)
                    {
                        case MessageBoxType.Ok: return MessageBoxResult.Cancel;
                        case MessageBoxType.OkCancel: return MessageBoxResult.Cancel;
                        case MessageBoxType.YesNo: return MessageBoxResult.No;
                        case MessageBoxType.YesNoCancel: return MessageBoxResult.No;
                        default: return MessageBoxResult.Cancel;
                    }
                default:
                    return MessageBoxResult.Cancel;
            }
        }

        private void SetupButtons(ContentDialog dialog)
        {
            switch (Type)
            {
                case MessageBoxType.Ok:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Ok));
                    break;
                case MessageBoxType.OkCancel:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Ok));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.Cancel));
                    break;
                case MessageBoxType.YesNo:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Yes));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.No));
                    break;
                case MessageBoxType.YesNoCancel:
                    dialog.SetPrimaryButton(Resolver.Resolve(ResourceTypes.Yes));
                    dialog.SetSecondaryButton(Resolver.Resolve(ResourceTypes.No));
                    dialog.SetCloseButton(Resolver.Resolve(ResourceTypes.Cancel));
                    break;
                default:
                    break;
            }
        }
    }
}
